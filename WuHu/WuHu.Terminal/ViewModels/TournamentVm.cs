using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    class TournamentVm : BaseVm
    {
        private Tournament _tournament;

        public TournamentVm(Tournament tournament)
        {
            _tournament = tournament;
        }
    }
}
