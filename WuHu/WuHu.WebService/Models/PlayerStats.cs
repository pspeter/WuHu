using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.WebService.Models
{
    [DataContract]
    public class PlayerStats
    {
        private readonly IMatchManager _matchManager = BLFactory.GetMatchManager();
        private readonly IPlayerManager _playerManager = BLFactory.GetPlayerManager();

        public PlayerStats() { }

        public PlayerStats(Player player)
        {
            if (player.PlayerId == null)
            {
                throw new ArgumentNullException(nameof(player.PlayerId));
            }

            PlayerId = player.PlayerId.Value;
            var matches = _matchManager.GetAllMatchesFor(player);

            // WinRate
            WinRate = (double) matches
                .Count(m => ((m.Player1.PlayerId == player.PlayerId || m.Player2.PlayerId == player.PlayerId) && m.ScoreTeam1 > m.ScoreTeam2) ||
                              (m.Player3.PlayerId == player.PlayerId || m.Player4.PlayerId == player.PlayerId) && m.ScoreTeam1 < m.ScoreTeam2)
                / matches.Count * 100; // won matches divided by all matches

            // CurrentScore
            CurrentScore = BLFactory.GetRatingManager().GetCurrentRatingFor(player).Value;

            // Most Played With
            var others = _playerManager.GetAllPlayers().Where(p => p.PlayerId != player.PlayerId);
            MostPlayedWithCount = 0;
            foreach (var other in others)
            {
                var playedWithCount = matches
                    .Count(m => m.Player1.PlayerId == other.PlayerId || m.Player2.PlayerId == other.PlayerId ||
                                m.Player3.PlayerId == other.PlayerId || m.Player4.PlayerId == other.PlayerId);
                if (playedWithCount > MostPlayedWithCount)
                {
                    MostPlayedWithCount = playedWithCount;
                    MostPlayedWith = other;
                }
            }

            // Last Match
            LastMatch = matches.OrderByDescending(m => m.Datetime).First();
        }

        [Required]
        [DataMember]
        public int PlayerId { get; set; }
        
        [DataMember]
        public int CurrentScore { get; set; }

        [DataMember]
        public double WinRate { get; set; }

        [DataMember]
        public Player MostPlayedWith { get; set; }
        
        [DataMember]
        public int MostPlayedWithCount { get; set; }

        [DataMember]
        public Match LastMatch { get; set; }
    }
}