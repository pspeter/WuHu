using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class RanklistVm : BaseVm
    {
        public RanklistVm(ObservableCollection<PlayerVm> players)
        {
            Players = players ?? new ObservableCollection<PlayerVm>();
            if (players == null)
            {
                LoadPlayers();
            }
        }

        public RanklistVm()
        {
            Players = new ObservableCollection<PlayerVm>();
            LoadPlayers();
        }

        public ObservableCollection<PlayerVm> Players { get; private set; }

        public ObservableCollection<PlayerVm> PlayersSortedByRank =>
            new ObservableCollection<PlayerVm>(Players.OrderByDescending(p => p.CurrentRating));

        private void LoadPlayers()
        {
            Players.Clear();
            var players = Manager.GetAllPlayers();

            foreach (var player in players)
            {
                Players.Add(new PlayerVm(player));
            }
            //var orderedPlayers = Players.OrderByDescending(p => p.CurrentRating);
        }
    }
}
