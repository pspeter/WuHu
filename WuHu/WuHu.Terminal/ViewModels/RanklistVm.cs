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
        public RanklistVm()
        {
            LoadPlayersAsync();
        }
    }
}
