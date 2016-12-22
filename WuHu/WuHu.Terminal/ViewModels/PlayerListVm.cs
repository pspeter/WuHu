using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Syncfusion.Windows.Shared;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerListVm : BaseVm
    {

        private PlayerVm _currentPlayer;

        public ICommand ShowAddPlayerCommand { get; }

        public PlayerListVm(Action<object>showAddPlayer)
        {
            ShowAddPlayerCommand = new RelayCommand(
                showAddPlayer,
                _ => IsAuthenticated);
            LoadPlayersAsync();
            OnPlayersLoaded += () =>
                CurrentPlayer = Players.Count > 0 ? Players.First() : null;
        }

        //public PlayerListVm(ObservableCollection<PlayerVm> players,
        //    ObservableCollection<PlayerVm> sortedPlayers) 
        //    : base(players, sortedPlayers)
        //{
        //    if (players != null)
        //    {
        //        CurrentPlayer = Players.Count > 0 ? Players.First() : null;
        //    }
        //    else
        //    {
        //        LoadPlayersAsync();
        //    }

        //    OnPlayersLoaded += () =>
        //        CurrentPlayer = Players.Count > 0 ? Players.First() : null;
        //}
        
        public PlayerVm CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (Equals(value, _currentPlayer)) return;
                _currentPlayer = value;
                OnPropertyChanged(this);
            }
        }
        

    }
}
