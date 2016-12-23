using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WuHu.BL;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public abstract class BaseVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action OnPlayersLoaded;
        public event Action OnPlayersSortedByRankLoaded;

        protected ITerminalManager AuthenticationManager;
        protected IRatingManager RatingManager;
        protected IPlayerManager PlayerManager;
        protected IMatchManager MatchManager;
        protected ITournamentManager TournamentManager;

        public bool IsAuthenticated => AuthenticationManager.IsUserAuthenticated();
        public bool IsNotAuthenticated => !AuthenticationManager.IsUserAuthenticated();

        protected virtual void OnAuthenticatedChanged(object sender)
        {
            OnPropertyChanged(sender, nameof(IsAuthenticated));
            OnPropertyChanged(sender, nameof(IsNotAuthenticated));
            OnPropertyChanged(sender, nameof(MatchVm.CanEdit));
        }

        protected virtual void OnPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        protected BaseVm() : this(null) { }

        protected BaseVm(ObservableCollection<PlayerVm> players = null,
            ObservableCollection<PlayerVm> sortedPlayers = null)
        {
            AuthenticationManager = ManagerFactory.GetTerminalManager();
            RatingManager = ManagerFactory.GetRatingManager();
            PlayerManager = ManagerFactory.GetPlayerManager();
            MatchManager = ManagerFactory.GetMatchManager();
            TournamentManager = ManagerFactory.GetTournamentManager();
            Players = players ?? new ObservableCollection<PlayerVm>();
            PlayersSortedByRank = sortedPlayers ?? new ObservableCollection<PlayerVm>();
        }
        
        public ObservableCollection<PlayerVm> Players { get; }
        public ObservableCollection<PlayerVm> PlayersSortedByRank { get; }

        protected async void LoadPlayersAsync()
        {

            var playerVms = await Task.Run(() => 
                PlayerManager.GetAllPlayers()
                .Select(player => new PlayerVm(player, Reload)).ToList());

            Players.Clear();
            foreach (var vm in playerVms)
            {
                Players.Add(vm); 
            }

            OnPlayersLoaded?.Invoke();

            var orderedPlayers = Players.OrderByDescending(p => p.CurrentRating?.Value ?? int.MinValue);
            PlayersSortedByRank.Clear();
            foreach (var player in orderedPlayers)
            {
                PlayersSortedByRank.Add(player);
            }

            OnPlayersSortedByRankLoaded?.Invoke();
        }

        public virtual void Reload()
        {
            LoadPlayersAsync();
        }
    }
}
