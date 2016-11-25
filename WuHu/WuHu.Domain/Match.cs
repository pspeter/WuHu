using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class Match
    {
        public Match(int? matchId, Tournament tournament, DateTime datetime, 
            byte? scoreteam1, byte? scoreteam2, float estimatedWinChance, bool isDone,
            Player player1, Player player2, Player player3, Player player4)
        {
            this.MatchId = matchId;
            this.Tournament = tournament;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.EstimatedWinChance = estimatedWinChance;
            this.IsDone = isDone;
            this.Player1 = player1;
            this.Player2 = player2;
            this.Player3 = player3;
            this.Player4 = player4;
        }

        public Match(Tournament tournament, DateTime datetime, byte? scoreteam1, 
            byte? scoreteam2, float estimatedWinChance, bool isDone,
            Player player1, Player player2, Player player3, Player player4)
        {
            if (player1.Equals(player2) || player1.Equals(player3) || player1.Equals(player4) ||
                player2.Equals(player3) || player2.Equals(player4) || player3.Equals(player4))
            {
                throw new ArgumentException("A Player can't play with or against himself");
            }
            if (estimatedWinChance > 1 || estimatedWinChance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(estimatedWinChance), 
                    "The estimated win chance should be between 0 and 1");
            }
            this.Tournament = tournament;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.EstimatedWinChance = estimatedWinChance;
            this.IsDone = isDone;
            this.Player1 = player1;
            this.Player2 = player2;
            this.Player3 = player3;
            this.Player4 = player4;
        }


        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player Player3 { get; set; }
        public Player Player4 { get; set; }
        public Tournament Tournament { get; set; }
        public int? MatchId { get; set; }
        public DateTime Datetime { get; set; }
        public byte? ScoreTeam1 { get; set; }
        public byte? ScoreTeam2 { get; set; }
        public float EstimatedWinChance { get; set; }
        public bool IsDone { get; set; }

    }
}
