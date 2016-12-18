using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerCollectionVm : INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        private PlayerVm _currentPlayer;
        private IPlayerManager _playerManager;

        public PlayerCollectionVm(IPlayerManager playerManager)
        {
            this._playerManager = playerManager;
            this.Players = new ObservableCollection<PlayerVm>();
            LoadPlayers();
        }

        public ObservableCollection<PlayerVm> Players { get; }

        private void LoadPlayers()
        {
            throw new NotImplementedException();
        }

        public PlayerVm CurrentFolder
        {
            get { return _currentPlayer; }
            set
            {
                if (Equals(value, _currentPlayer)) return;
                _currentPlayer = value;
                OnPropertyChanged();
            }
        }
        
        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, IList<PlayerVm> items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, items));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
