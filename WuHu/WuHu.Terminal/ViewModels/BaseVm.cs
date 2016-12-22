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

        protected ITerminalManager Manager;

        public bool IsAuthenticated => Manager.IsUserAuthenticated();
        public bool IsNotAuthenticated => !Manager.IsUserAuthenticated();

        protected void OnAuthenticatedChanged(object sender)
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
            Manager = ManagerFactory.GetTerminalManager();
            Players = players ?? new ObservableCollection<PlayerVm>();
            PlayersSortedByRank = sortedPlayers ?? new ObservableCollection<PlayerVm>();
        }
        
        public ObservableCollection<PlayerVm> Players { get; }
        public ObservableCollection<PlayerVm> PlayersSortedByRank { get; }

        protected async void LoadPlayersAsync()
        {
            Players.Clear();

            var playerVms = await Task.Run(() => 
                Manager.GetAllPlayers()
                .Select(player => new PlayerVm(player, Reload)).ToList());

            foreach (var vm in playerVms)
            {
                Players.Add(vm); 
            }

            OnPlayersLoaded?.Invoke();

            var orderedPlayers = Players.OrderByDescending(p => p.CurrentRating?.Value ?? int.MinValue);
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
