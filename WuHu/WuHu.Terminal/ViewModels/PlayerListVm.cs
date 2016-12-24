using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerListVm : BaseVm
    {

        private PlayerVm _currentPlayer;
        private readonly Action _reloadTabs;

        public ICommand ShowAddPlayerCommand { get; }

        public PlayerListVm(Action<object>showAddPlayer, Action reloadTabs, Action<string> queueMessage)
        {
            ShowAddPlayerCommand = new RelayCommand(
                showAddPlayer,
                _ => IsAuthenticated);
            LoadPlayersAsync();
            OnPlayersLoaded += () =>
                CurrentPlayer = Players.Count > 0 ? Players.First() : null;
            _reloadTabs = reloadTabs;
        }
        
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

        public override void Reload()
        {
            base.Reload();
            _reloadTabs.Invoke();
        }
    }
}
