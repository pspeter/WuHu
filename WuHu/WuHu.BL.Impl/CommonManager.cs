using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL.Impl;
using WuHu.Common;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class CommonManager : ICommonManager
    {
        protected readonly Authenticator Authentication;
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        protected CommonManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
            Authentication = Authenticator.GetInstance();
        }

        // Match
        public bool SetScore(Match match, Credentials credentials)
        {
            if (!Authenticate(credentials, true) || match.MatchId == null)
            {
                return false;
            }

            var oldMatch = MatchDao.FindById(match.MatchId.Value);

            if (oldMatch.IsDone)
            {
                return false;
            }
           
            oldMatch.ScoreTeam1 = match.ScoreTeam1;
            oldMatch.ScoreTeam2 = match.ScoreTeam2;
            oldMatch.IsDone = true;
            var updated = MatchDao.Update(oldMatch);

            if (!updated)
            {
                return false;
            }

            AddCurrentRatingFor(match.Player1, credentials);
            AddCurrentRatingFor(match.Player2, credentials);
            AddCurrentRatingFor(match.Player3, credentials);
            AddCurrentRatingFor(match.Player4, credentials);
            return true;
        }

        public IList<Match> GetAllMatches()
        {
            return MatchDao.FindAll();
        }

        public IList<Match> GetAllMatchesFor(Player player)
        {
            return MatchDao.FindAllByPlayer(player);
        }

        public IList<Match> GetAllMatchesFor(Tournament tournament)
        {
            return MatchDao.FindAllByTournament(tournament);
        }

        public IList<Match> GetAllUnfinishedMatches()
        {
            return new List<Match>(MatchDao.FindAll().Where(m => !m.IsDone));
        }

        // Player
        public bool AddPlayer(Player player, Credentials credentials)
        {
            if (!Authenticate(credentials, true)) // only admins can add players
            {
                return false;
            }
            var inserted = PlayerDao.Insert(player);
            if (inserted)
            {
                AddCurrentRatingFor(player, credentials);
            }
            return inserted;
        }

        public bool UpdatePlayer(Player player, Credentials credentials)
        {
            if (player.PlayerId == null || credentials == null)
            {
                return false;
            }

            // only admins can update other players and users can't make themselves admin
            if (credentials.Username != player.Username || player.IsAdmin)
            {
                return Authenticate(credentials, true) && PlayerDao.Update(player);
            }
            // normal users can edit themselves
            return Authenticate(credentials, false) && PlayerDao.Update(player);
        }

        public bool ChangePassword(string username, string newPassword, Credentials credentials)
        {
            var player = PlayerDao.FindByUsername(username);

            if ((credentials.Username != player.Username && Authenticate(credentials, true)) ||
                (credentials.Username == player.Username && Authenticate(credentials, false)))
            {
                player.Salt = CryptoService.GenerateSalt();
                player.Password = CryptoService.HashPassword(newPassword, player.Salt);
                return true;
            }
            return false;
        }

        public Player GetPlayer(int playerId)
        {
            return PlayerDao.FindById(playerId);
        }

        public Player GetPlayer(string username)
        {
            return PlayerDao.FindByUsername(username);
        }

        public IList<Player> GetAllPlayers()
        {
            return PlayerDao.FindAll();
        }

        public IList<Player> GetRanklist()
        {
            return new List<Player>(
                PlayerDao.FindAll()
                .OrderByDescending(p => RatingDao.FindCurrentRating(p)));
        }


        // Rating
        public bool AddCurrentRatingFor(Player player, Credentials credentials)
        {
            if (!Authenticate(credentials, true)) // only admins can add players
            {
                return false;
            }

            var kRating = int.Parse(ParamDao.FindById("kRating")
                ?.Value ?? DefaultParameter.KRating);
            var halflife = int.Parse(ParamDao.FindById("halflife")
                ?.Value ?? DefaultParameter.Halflife);
            var initialScore = int.Parse(ParamDao.FindById("initialScore")
                ?.Value ?? DefaultParameter.InitialScore);

            if (RatingDao.FindCurrentRating(player) == null)
            {
                RatingDao.Insert(new Rating(player, DateTime.Now, initialScore));
                return true;
            }

            var matches = MatchDao.FindAllByPlayer(player)
                .Where(m => m.IsDone)
                .OrderBy(m => m.Datetime);

            var rating = initialScore;
            var matchNr = matches.Count() - 1;
            foreach (var match in matches)
            {
                if (match.ScoreTeam1 == null || match.ScoreTeam2 == null)
                {
                    throw new ArgumentException("Player has finished matches without a score");
                }

                var multiplier = Math.Pow(0.5, (double)matchNr / halflife);

                int playerWon;
                double winChance;

                if (player.Username == match.Player1.Username ||
                    player.Username == match.Player2.Username)
                {
                    playerWon = match.ScoreTeam1.Value > match.ScoreTeam2.Value ? 1 : 0;
                    winChance = match.EstimatedWinChance;
                }
                else
                {
                    playerWon = match.ScoreTeam1.Value < match.ScoreTeam2.Value ? 1 : 0;
                    winChance = 1 - match.EstimatedWinChance;
                }

                var deltaScore = kRating * (playerWon - winChance);
                rating += (int)Math.Floor(multiplier * deltaScore);
                --matchNr;
            }

            return RatingDao.Insert(new Rating(player, DateTime.Now, rating));
        }

        public IList<Rating> GetAllRatings()
        {
            return RatingDao.FindAll();
        }

        public IList<Rating> GetAllRatingsFor(Player player)
        {
            return RatingDao.FindAllByPlayer(player);
        }

        public Rating GetCurrentRatingFor(Player player)
        {
            return RatingDao.FindCurrentRating(player);
        }


        // Scoreparameter
        public IList<ScoreParameter> GetAllParameters()
        {
            return ParamDao.FindAll();
        }

        public bool AddParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            return ParamDao.Insert(param);
        }

        public bool UpdateParameter(ScoreParameter param, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            return ParamDao.Update(param);
        }

        public ScoreParameter GetParameter(string key)
        {
            return ParamDao.FindById(key);
        }


        // Tournament

        public IList<Tournament> GetAllTournaments()
        {
            return TournamentDao.FindAll();
        }

        public Tournament GetMostRecentTournament()
        {
            return TournamentDao.FindMostRecentTournament();
        }

        public bool LockTournament(Credentials credentials)
        {
            if (!Authenticate(credentials, true) || TournamentLocked)
            {
                return false;
            }
            return TournamentLocked = true;
        }

        public void UnlockTournament(Credentials credentials)
        {
            if (Authenticate(credentials, true))
            {
                TournamentLocked = false;
            }
        }

        public bool CreateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials)
        {
            if (!Authenticate(credentials, true))
            {
                return false;
            }
            var inserted = TournamentDao.Insert(tournament);
            if (!inserted)
            {
                return false;
            }
            LockTournament(credentials);
            SetMatchesForTournament(tournament, players, amountMatches, credentials);
            UnlockTournament(credentials);
            return true;
        }


        public bool UpdateTournament(Tournament tournament, IList<Player> players, int amountMatches, Credentials credentials)
        {
            return SetMatchesForTournament(tournament, players, amountMatches, credentials);
        }

        private bool SetMatchesForTournament(Tournament tournament, IList<Player> players, int amount, Credentials credentials)
        {
            if (!Authenticate(credentials, true) || !TournamentLocked ||
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
                var playerIndex1 = Rand.Next(players.Count());
                var player1 = players.ElementAt(playerIndex1);
                players.RemoveAt(playerIndex1);
                var player1Rating = RatingDao.FindCurrentRating(player1)?.Value ?? 1000;

                // as his first opponent, pick somebody with similar rating
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                }
                var player3 = PickPlayer(players, player1Rating);
                var player3Rating = RatingDao.FindCurrentRating(player3)?.Value ?? 1000;

                // as his teammate, pick somebody who would even out their score difference
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player3);
                }
                var player2 = PickPlayer(players, 2 * player3Rating - player1Rating);
                var player2Rating = RatingDao.FindCurrentRating(player2)?.Value ?? 1000;

                // as his second opponent, again try to cancel out the rating difference between the teams
                if (players.Count == 0)
                {
                    players = new List<Player>(playersCopy);
                    players.Remove(player1);
                    players.Remove(player2);
                    players.Remove(player3);
                }
                var player4 = PickPlayer(players, player1Rating + player2Rating - player3Rating);
                var player4Rating = RatingDao.FindCurrentRating(player4)?.Value ?? 1000;

                var team1Rating = ((double)player1Rating + player2Rating) / 2;
                var team2Rating = ((double)player3Rating + player4Rating) / 2;

                var estimatedWinChance = 1 / (1 + Math.Pow(10, (team2Rating - team1Rating) / 400));

                var newMatch = new Match(tournament, DateTime.Now, null, null, estimatedWinChance,
                    false, player1, player2, player3, player4);

                MatchDao.Insert(newMatch);
            }
            UnlockTournament(credentials);
            return true;
        }

        private Player PickPlayer(ICollection<Player> players, int fromScore)
        {
            var playerCount = players.Count; // faster than LINQ Count() on sortedPlayers

            var sortedPlayers = players.OrderBy(p => Math.Abs(RatingDao.FindCurrentRating(p)?.Value ?? 1000 - fromScore));

            var cutoff = 1 / (Math.Log(playerCount) + 1);

            foreach (var player in sortedPlayers)
            {
                if (Rand.NextDouble() < cutoff)
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
            var matchesToCancel = MatchDao
                .FindAllByTournament(tournament)
                .Where(match => !match.IsDone);

            foreach (var match in matchesToCancel)
            {
                MatchDao.Delete(match);
            }
        }

        private bool Authenticate(Credentials credentials, bool adminRequired)
        {
            return Authentication?.Authenticate(credentials, adminRequired) ?? false;
        }
    }
}
