using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class Match
    {
        
        public Match() { }


        public Match(int? matchId, int tournamentId, DateTime datetime, byte? scoreteam1, byte? scoreteam2, float estimatedWinChance, bool isDone)
        {
            this.MatchId = matchId;
            this.TournamentId = tournamentId;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.EstimatedWinChance = estimatedWinChance;
            this.IsDone = isDone;
        }
        
        public int TournamentId { get; set; }
        public int? MatchId { get; }
        public DateTime Datetime { get; set; }
        public byte? ScoreTeam1 { get; set; }
        public byte? ScoreTeam2 { get; set; }
        public float EstimatedWinChance { get; set; }
        public bool IsDone { get; set; }

    }
}
