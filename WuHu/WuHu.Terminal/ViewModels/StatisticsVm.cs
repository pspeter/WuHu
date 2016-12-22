using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Terminal.ViewModels
{
    public class StatisticsVm : BaseVm
    {
        public IEnumerable<Tuple<DateTime, int>> ChartData;

        public StatisticsVm()
        {

        }
    }
}
