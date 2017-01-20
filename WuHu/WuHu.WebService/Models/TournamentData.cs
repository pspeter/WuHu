using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.WebService.Models
{
    [DataContract]
    public class TournamentData
    {
        public TournamentData() { }

        public TournamentData(Tournament tournament)
        {
            if (tournament?.TournamentId == null)
            {
                throw new ArgumentNullException(nameof(tournament.TournamentId));
            }
            TournamentId = tournament.TournamentId.Value;
            Name = tournament.Name;
            Datetime = tournament.Datetime;
            Players = null;
            Amount = 1;
        }
        
        [DataMember]
        public int TournamentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime Datetime { get; set; }

        [DataMember]
        [Required]
        public IList<Player> Players { get; set; }

        [DataMember]
        [Required]
        public int Amount { get; set; }
    }
}