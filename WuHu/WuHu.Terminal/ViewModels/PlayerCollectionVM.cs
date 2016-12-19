using System.Collections.ObjectModel;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerCollectionVm : BaseVm
    {
        private PlayerVm _currentPlayer;

        public PlayerCollectionVm()
        {
            Players = new ObservableCollection<PlayerVm>();
            LoadPlayers();
        }

        public ObservableCollection<PlayerVm> Players { get; }

        private void LoadPlayers()
        {
            Players.Clear();
            var players = Manager.GetAllPlayers();

            foreach (var player in players) { 
                Players.Add(new PlayerVm(player));
            }
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
    }
}
