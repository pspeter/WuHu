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
    public class RanklistData
    {
        public RanklistData() { }

        public RanklistData(Player player)
        {
            Name = player.ToString();
            CurrentScore = BLFactory.GetRatingManager().GetCurrentRatingFor(player).Value;
            var matches = BLFactory.GetMatchManager().GetAllMatchesFor(player);

            if (matches.Count == 0)
            {
                WinRate = 0;
            }
            else
            {
                WinRate = (double)matches
                    .Count(m => ((m.Player1.PlayerId == player.PlayerId || m.Player2.PlayerId == player.PlayerId) && m.ScoreTeam1 > m.ScoreTeam2) ||
                                  (m.Player3.PlayerId == player.PlayerId || m.Player4.PlayerId == player.PlayerId) && m.ScoreTeam1 < m.ScoreTeam2)
                    / matches.Count * 100; // won matches divided by all matches
            }
        }

        [Required]
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public int CurrentScore { get; set; }

        [DataMember]
        public double WinRate { get; set; }
    }
}