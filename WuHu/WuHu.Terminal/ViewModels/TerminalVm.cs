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
using MaterialDesignThemes.Wpf;
using WuHu.Terminal.Services;
using WuHu.Terminal.ViewModels;
using WuHu.Terminal.Views;

namespace WuHu.Terminal.ViewModels
{
    public class TerminalVm : BaseVm
    {
        public ICommand LogoutCommand { get; private set; }
        public ISnackbarMessageQueue MessageQueue { get; set; }

        public ObservableCollection<TabItemVm> TabItems { get; }
        private readonly TabItemVm _authenticationTab;
        private readonly TabItemVm _playerTab;
        private readonly TabItemVm _statTab;

        public TerminalVm()
        {
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

            var rankListVm = new RanklistVm();
            var authenticationVm = new AuthenticationVm(msg =>
            {
                OnAuthenticatedChanged(this);
                UpdateAuthHeader();
                MessageQueue.Enqueue(msg);
            });
            var statisticsVm = new StatisticsVm();
            var playerPageVm = new PlayerPageVm(() =>
            {
                statisticsVm.Reload();
                rankListVm.Reload();
            },
            MessageQueue.Enqueue);

            var tournamentVm = new TournamentVm(() =>
            {
                statisticsVm.Reload();
                rankListVm.Reload();
            }, 
            MessageQueue.Enqueue);

            _authenticationTab = new TabItemVm("LOGIN", "LoginVariant", authenticationVm);
            _playerTab = new TabItemVm("SPIELER", "AccountCardDetails", playerPageVm);
            _statTab = new TabItemVm("STATISTIK", "TrendingUp", statisticsVm);
            _playerTab.IsEnabled = IsAuthenticated;
            _statTab.IsEnabled = IsAuthenticated;

            TabItems = new ObservableCollection<TabItemVm>
            {
                new TabItemVm("RANGLISTE", "Trophy", rankListVm),
                new TabItemVm("SPIELPLAN", "FormatListBulleted", tournamentVm),
                _playerTab,
                _statTab,
                _authenticationTab
            };
        }

        private void UpdateAuthHeader()
        {
            _authenticationTab.Header = IsAuthenticated ? "LOGOUT" : "LOGIN";
            _authenticationTab.IconName = IsAuthenticated ? "LogoutVariant" : "LoginVariant";
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            AuthenticationService.Logout();
            OnAuthenticatedChanged(this);
        }

        protected override void OnAuthenticatedChanged(object sender)
        {
            OnPropertyChanged(sender, nameof(IsAuthenticated));
            OnPropertyChanged(sender, nameof(IsNotAuthenticated));
            OnPropertyChanged(sender, nameof(MatchVm.CanEdit));
            _playerTab.IsEnabled = IsAuthenticated;
            _statTab.IsEnabled = IsAuthenticated;
        }
    }
}
