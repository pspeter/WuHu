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
    public class RanklistVm : PlayerCollectionVm
    {
        public RanklistVm(ObservableCollection<PlayerVm> players,
            ObservableCollection<PlayerVm> sortedPlayers) : base(players, sortedPlayers)
        { }

        public RanklistVm() : this(null, null)
        { }
    }
}
