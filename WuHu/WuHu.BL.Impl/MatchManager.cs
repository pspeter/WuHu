using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL.Impl;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class MatchManager : IMatchManager
    {
        private static MatchManager _instance;
        private readonly Authenticator _authenticator;
        private readonly IMatchDao _matchDao;
        private readonly IRatingDao _ratingDao;
        private readonly IScoreParameterDao _paramDao;
        private bool _isLocked;
        private readonly Random _rand;

        protected MatchManager()
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _authenticator = Authenticator.GetInstance();
            _rand = new Random();
        }

        public static MatchManager GetInstance()
        {
            return _instance ?? (_instance = new MatchManager());
        }

        public bool Lock()
        {
            if (_isLocked)
            {
                return false;
            }
            return _isLocked = true;
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        public bool CreateMatches(Tournament tournament, IList<Player> players, int amount, Credentials credentials)
        {
            if (!Authenticate(credentials, true) || _isLocked || 
                players.Count() < 4 || amount < 1)
            {
                return false;
            }

            // if any matches that haven't been played exist, cancel them
            CancelMatches(tournament);

            var playersCopy = new List<Player>(players);
            for (var matchNr = 0; matchNr < amount; ++matchNr)
            {
                // first, pick a random player
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                }
                var playerIndex1 = _rand.Next(players.Count());
                var player1 = players.ElementAt(playerIndex1);
                players.RemoveAt(playerIndex1);
                var player1Rating = _ratingDao.FindCurrentRating(player1).Value;

                // as his first opponent, pick somebody with similar rating
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                }
                var player3 = PickPlayer(players, player1Rating);
                var player3Rating = _ratingDao.FindCurrentRating(player3).Value;

                // as his teammate, pick somebody who would even out their score difference
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player3);
                }
                var player2 = PickPlayer(players, 2*player3Rating - player1Rating);
                var player2Rating = _ratingDao.FindCurrentRating(player2).Value;

                // as his second opponent, again try to cancel out the rating difference between the teams
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player2);
                    players.Remove(player3);
                }
                var player4 = PickPlayer(players, player1Rating + player2Rating - player3Rating);
                var player4Rating = _ratingDao.FindCurrentRating(player4).Value;

                var team1Rating = ((double) player1Rating + player2Rating)/2;
                var team2Rating = ((double) player3Rating + player4Rating)/2;

                var estimatedWinChance = 1/(1 + Math.Pow(10, (team2Rating - team1Rating)/400));

                var newMatch = new Match(tournament, DateTime.Now, null, null, estimatedWinChance,
                    false, player1, player2, player3, player4);

                if (!_matchDao.Insert(newMatch))
                {
                    Unlock();
                    return false;
                };
            }
            Unlock();
            return true;
        }

        private Player PickPlayer(ICollection<Player> players, int fromScore)
        {
            var playerCount = players.Count; // faster than LINQ Count() on sortedPlayers
            var sortedPlayers = players.OrderBy(p => Math.Abs(_ratingDao.FindCurrentRating(p).Value - fromScore));

            var cutoff = 1/(Math.Log(playerCount) + 1);

            foreach (var player in sortedPlayers)
            { 
                if (_rand.NextDouble() < cutoff)
                {
                    players.Remove(player);
                    return player;
                }
            }
            var returnPlayer = sortedPlayers.First();
            players.Remove(returnPlayer);
            return returnPlayer;
        } 

        private void CancelMatches(Tournament tournament)
        {
            var matchesToCancel = _matchDao
                .FindAllByTournament(tournament)
                .Where(match => !match.IsDone);

            foreach (var match in matchesToCancel)
            {
                _matchDao.Delete(match);
            }
        }

        public bool SetScore(Match match, Credentials credentials)
        {
            if (!Authenticate(credentials, true) || match.MatchId == null)
            {
                return false;
            }

            var oldMatch = _matchDao.FindById(match.MatchId.Value);

            if (oldMatch.IsDone)
            {
                return false;
            }

            var ratingMgr = RatingManager.GetInstance();
            oldMatch.ScoreTeam1 = match.ScoreTeam1;
            oldMatch.ScoreTeam2 = match.ScoreTeam2;
            oldMatch.IsDone = true;
            var updated = _matchDao.Update(oldMatch);

            if (!updated)
            {
                return false;
            }

            ratingMgr.AddCurrentRatingFor(match.Player1, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player2, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player3, credentials);
            ratingMgr.AddCurrentRatingFor(match.Player4, credentials);
            return true;
        }

        public IList<Match> GetAllMatches()
        {
            return _matchDao.FindAll();
        }

        public IList<Match> GetAllMatchesFor(Player player)
        {
            return _matchDao.FindAllByPlayer(player);
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return _authenticator.Authenticate(credentials, adminRequired);
        }
    }
}
