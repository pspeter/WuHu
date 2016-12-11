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
        private Authenticator _authenticator;
        private readonly IMatchDao _matchDao;
        private readonly IRatingDao _ratingDao;
        private readonly IScoreParameterDao _paramDao;
        private bool _isLocked;
        private Random _rand;

        protected MatchManager()
        {
            var database = DalFactory.CreateDatabase();
            _matchDao = DalFactory.CreateMatchDao(database);
            _ratingDao = DalFactory.CreateRatingDao(database);
            _paramDao = DalFactory.CreateScoreParameterDao(database);
            _authenticator = Authenticator.GetInstance();
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

            var kRating = int.Parse(_paramDao.FindById("k-rating").Value);
            _rand = new Random();

            var playersCopy = new List<Player>(players);
            for (int matchNr = 0; matchNr < amount; ++matchNr)
            {
                Player player1, player2, player3, player4;
                // first, pick a random player
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                }
                var playerIndex1 = _rand.Next(players.Count());
                player1 = players[playerIndex1];
                players.RemoveAt(playerIndex1);
                var player1Rating = _ratingDao.FindCurrentRating(player1).Value;

                // as his first opponent, pick somebody with similar rating
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                }
                player3 = PickPlayer(players, player1Rating);
                var player3Rating = _ratingDao.FindCurrentRating(player3).Value;

                // as his teammate, pick somebody who would even out their score difference
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player3);
                }
                player2 = PickPlayer(players, 2*player3Rating - player1Rating);
                var player2Rating = _ratingDao.FindCurrentRating(player2).Value;

                // as his second opponent, again try to cancel out the rating difference between the teams
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player2);
                    players.Remove(player3);
                }
                player4 = PickPlayer(players, player1Rating + player2Rating - player3Rating);
                var player4Rating = _ratingDao.FindCurrentRating(player4).Value;

                var team1Rating = ((double) player1Rating + player2Rating)/2;
                var team2Rating = ((double) player3Rating + player4Rating)/2;

                var estimatedWinChance = 1/(1 + Math.Pow(10, (team2Rating - team1Rating)/400));

                var newMatch = new Match(tournament, DateTime.Now, null, null, estimatedWinChance,
                    false, player1, player2, player3, player4);

                _matchDao.Insert(newMatch);
            }

            return false;
        }

        private Player PickPlayer(IList<Player> players, int fromScore)
        {
            var chances = CalculatePickChances(players, fromScore);
            double randChance = _rand.NextDouble() * chances.Sum(); // second player random Nr

            int playerIndex2 = 0;
            foreach (var chance in chances)
            {
                randChance -= chance;
                if (randChance <= 0)
                {
                    players.RemoveAt(playerIndex2);
                    return players[playerIndex2];
                }
            }
            return players.Last();
        } 

        private IEnumerable<double> CalculatePickChances(IList<Player> players, int fromScore)
        {
            var ratings = players.Select(p => _ratingDao.FindCurrentRating(p).Value);

            var scoreDifferences = ratings.Select(rating => Math.Abs(rating - fromScore));

            var minDiff = scoreDifferences.Min();
            var maxDiff = scoreDifferences.Max();
            var k = 1/Math.Log(maxDiff - minDiff + 1);
            
            return scoreDifferences.Select(s => 1 - Math.Log(s - minDiff + 1)*k);
        }

        private void CancelMatches(Tournament tournament)
        {
            IEnumerable<Match> matchesToCancel = _matchDao
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
