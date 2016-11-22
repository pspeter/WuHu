using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
    [Serializable]
    public class Rating
    {
        public Rating(int? ratingId, Player player, DateTime datetime, int value)
        {
            this.Player = player;
            this.Datetime = datetime;
            this.Value = value;
        }
        
        public int? RatingId { get; set; }
        public Player Player { get; set; }
        public DateTime Datetime { get; set; }
        public int Value { get; set; }
    }
}
