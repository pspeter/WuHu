using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    class Match
    {
        
        public Match() { }

        public Match(Tournament tournament, DateTime datetime, byte scoreteam1, byte scoreteam2, short deltaPoints, bool isDone)
        {
            this.Tournament = tournament;
            this.ScoreTeam1 = scoreteam1;
            this.ScoreTeam2 = scoreteam2;
            this.DeltaPoints = deltaPoints;
            this.IsDone = isDone;
        }
        
        public Tournament Tournament { get; set; }
        public long MatchId { get; }
        public DateTime Datetime { get; set; }
        public byte ScoreTeam1 { get; set; }
        public byte ScoreTeam2 { get; set; }
        public short DeltaPoints { get; set; }
        public bool IsDone { get; set; }

    }
}
