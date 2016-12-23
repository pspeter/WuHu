﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Domain;

namespace WuHu.BL.Impl
{
    public class TournamentManager : ITournamentManager
    {
        protected readonly Authenticator Authentication;
        protected readonly IMatchDao MatchDao;
        protected readonly IPlayerDao PlayerDao;
        protected readonly ITournamentDao TournamentDao;
        protected readonly IScoreParameterDao ParamDao;
        protected readonly IRatingDao RatingDao;
        protected readonly Random Rand = new Random();
        protected bool TournamentLocked;

        public TournamentManager()
        {
            var database = DalFactory.CreateDatabase();
            MatchDao = DalFactory.CreateMatchDao(database);
            PlayerDao = DalFactory.CreatePlayerDao(database);
            RatingDao = DalFactory.CreateRatingDao(database);
            TournamentDao = DalFactory.CreateTournamentDao(database);
            ParamDao = DalFactory.CreateScoreParameterDao(database);
            Authentication = Authenticator.GetInstance();
        }


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
            if (!Authenticate(credentials, true) || amountMatches < 1 || players.Count < 4)
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
