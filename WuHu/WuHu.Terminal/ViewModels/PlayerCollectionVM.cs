using System.Collections.ObjectModel;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerCollectionVm : RanklistVm
    {
        public PlayerCollectionVm()
        { }

        public PlayerCollectionVm(ObservableCollection<PlayerVm> players = null) : base(players)
        { }

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
