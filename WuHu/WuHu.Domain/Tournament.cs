using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [DataContract(Namespace = "http://WuHu.Domain")]
    public class Tournament
    {
        public Tournament() { }
        public Tournament(int tournamentId, string name, DateTime datetime, bool isLocked = false)
        {
            TournamentId = tournamentId;
            Name = name;
            Datetime = datetime;
            IsLocked = isLocked;
        }

        public Tournament(string name, DateTime datetime, bool isLocked = false)
        {
            Name = name;
            Datetime = datetime;
            IsLocked = isLocked;
        }

        [DataMember]
        public int? TournamentId { get; set; }
        [DataMember]
        public string Name { get;  set; }
        [DataMember]
        [Required]
        public DateTime Datetime { get; set; }
        [DataMember]
        public bool IsLocked { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
