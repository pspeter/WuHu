using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class Tournament
    {

        public Tournament() { }

        public Tournament(int tournamentId, string name, int creatorId)
        {
            this.TournamentId = tournamentId;
            this.Name = name;
            this.Creator = creatorId;
        }
       
        public int TournamentId { get; set; }
        public string Name { get;  set; }
        public int Creator { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
