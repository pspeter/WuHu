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
    public class Rating
    {
        public Rating() { }
        public Rating(int ratingId, Player player, DateTime datetime, int value)
        {
            this.RatingId = ratingId;
            this.Player = player;
            this.Datetime = datetime;
            this.Value = value;
        }

        public Rating(Player player, DateTime datetime, int value)
        {
            this.Player = player;
            this.Datetime = datetime;
            this.Value = value;
        }

        [DataMember]
        public int? RatingId { get; set; }
        [DataMember]
        [Required]
        public Player Player { get; set; }
        [DataMember]
        [Required]
        public DateTime Datetime { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}
