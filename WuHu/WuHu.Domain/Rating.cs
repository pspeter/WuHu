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
        public Rating() { }
        public Rating(int? ratingId, int playerId, DateTime datetime, int value)
        {
            this.RatingId = ratingId;
            this.PlayerId = playerId;
            this.Datetime = datetime;
            this.Value = value;
        }

        public int? RatingId { get; set; }
        public int PlayerId { get; set; }
        public DateTime Datetime { get; set; }
        public int Value { get; set; }
    }
}
