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
        public Tournament(int tournamentId, string name, DateTime datetime)
        {
            this.TournamentId = tournamentId;
            this.Name = name;
            this.Datetime = datetime;
        }

        public Tournament(string name, DateTime datetime)
        {
            this.Name = name;
            this.Datetime = datetime;
        }

        public int? TournamentId { get; set; }
        public string Name { get;  set; }
        public DateTime Datetime { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
