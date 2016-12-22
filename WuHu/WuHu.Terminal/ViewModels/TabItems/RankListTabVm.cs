using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Terminal.ViewModels.TabItems
{
    public class RankListTabVm : TabItemVm
    {
        //public RanklistVm Content { get; }

        public RankListTabVm(string name, string iconname, RanklistVm vm) : base(name, iconname, vm)
        {
            //Content = vm;
        }
    }
}
