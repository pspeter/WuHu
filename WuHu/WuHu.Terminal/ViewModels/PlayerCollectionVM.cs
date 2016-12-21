using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerCollectionVm : BaseVm
    {
        public PlayerCollectionVm()
        {
            LoadPlayersAsync();
        }

        public PlayerCollectionVm(ObservableCollection<PlayerVm> players,
            ObservableCollection<PlayerVm> sortedPlayers) 
            : base(players, sortedPlayers)
        {
            if (players != null)
            {
                CurrentPlayer = Players.Count > 0 ? Players.First() : null;
            }
            else
            {
                LoadPlayersAsync();
            }
        }
        
        private PlayerVm _currentPlayer;
        
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
