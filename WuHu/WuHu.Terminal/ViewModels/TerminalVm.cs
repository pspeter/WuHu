using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls;
using WuHu.Terminal.ViewModels.TabItems;
using WuHu.Terminal.Views;

namespace WuHu.Terminal.ViewModels
{
    public class TerminalVm : BaseVm
    {
        public ICommand LogoutCommand { get; private set; }

        public ObservableCollection<TabItemVm> TabItems { get; }

        private RanklistVm _rankListVm;
        private TournamentVm _tournamentVm;

        public TerminalVm()
        {
            // commands
            LogoutCommand = new RelayCommand(
                Logout,
                p => IsAuthenticated);

            _rankListVm = new RanklistVm();
            _tournamentVm = new TournamentVm();

            TabItems = new ObservableCollection<TabItemVm>
            {
                new TabItemVm("RANGLISTE", "Trophy", _rankListVm),
                new TabItemVm("SPIELPLAN", "FormatListBulleted", _tournamentVm),
                new TabItemVm("SPIELER", "AccountCardDetails", new PlayerPageVm()),
                new TabItemVm("STATISTIKEN", "TrendingUp", new StatisticsVm()),
                new TabItemVm("LOGIN", "LoginVariant", new LoginVm())
            };
        }
        

        private void Logout(object o)
        {
            Manager.Logout();
            OnAuthenticatedChanged(this);
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Manager.Logout();
            OnAuthenticatedChanged(this);
        }
    }
}
