using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
    public class TerminalVm : BaseVm
    {
        private BaseVm _currentVm;
        private RanklistVm _ranklistVm = new RanklistVm();
        private PlayerCollectionVm _playersVm = new PlayerCollectionVm();
        
        public ObservableCollection<PlayerVm> Players { get; }

        public BaseVm CurrentVm
        {
            get { return _currentVm; }
            set
            {
                if (CurrentVm != value)
                {
                    _currentVm = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public ICommand ShowRanklistCommand { get; private set; }
        public ICommand ShowPlayerlistCommand { get; private set; }

        public TerminalVm()
        { 
            // centrally saved players to reduce loading times for tab switching
            Players = new ObservableCollection<PlayerVm>();
            LoadPlayers();

            // button commands
            ShowRanklistCommand = new RelayCommand(p => CurrentVm = _ranklistVm);
            ShowPlayerlistCommand = new RelayCommand(
                p => CurrentVm = _playersVm);

            // starting view
            CurrentVm = _ranklistVm;
        }
        
        private void LoadPlayers()
        {
            Players.Clear();
            var players = Manager.GetAllPlayers().OrderByDescending(p => Manager.GetCurrentRatingFor(p)?.Value ?? 0);
            
            foreach (var player in players)
            {
                Players.Add(new PlayerVm(player));
            }
        }
    }
}
