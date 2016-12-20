using System.Collections.ObjectModel;
using System.Linq;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerCollectionVm : RanklistVm
    {
        public PlayerCollectionVm()
        {
            CurrentPlayer = Players.First();
        }

        public PlayerCollectionVm(ObservableCollection<PlayerVm> players) : base(players)
        {
            CurrentPlayer = Players.First();
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
