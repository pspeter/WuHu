using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class TournamentVm : BaseVm
    {
        private readonly Tournament _tournament;
        private int _amountMatches;
        private MatchVm _currentMatch;

        public ObservableCollection<MatchVm> Matches { get; }

        public TournamentVm()
        {
            _tournament = Manager.GetMostRecentTournament();
            Matches = new ObservableCollection<MatchVm>();

            LoadMatchesForTournamentAsync(_tournament);
            SetMatchesCommand = new RelayCommand(param => SetMatches());

            LoadPlayersAsync();
        }

        public TournamentVm(ObservableCollection<PlayerVm> players) : base(players)
        {
            _tournament = Manager.GetMostRecentTournament();
            Matches = new ObservableCollection<MatchVm>();
            LoadMatchesForTournamentAsync(_tournament);
            SetMatchesCommand = new RelayCommand(param => SetMatches());
        }

        public MatchVm CurrentMatch
        {
            get { return _currentMatch; }
            set
            {
                if (_currentMatch == value)
                {
                    _currentMatch = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public string Name
        {
            get { return _tournament.Name; }
            set
            {
                if (Equals(_tournament.Name, value)) return;
                _tournament.Name = value;
                OnPropertyChanged(this);
            }
        }

        public int AmountMatches
        {
            get { return _amountMatches; }
            set
            {
                if (_amountMatches == value) return;
                _amountMatches = value;
                OnPropertyChanged(this);
            }
        }

        public ICommand SetMatchesCommand { get; }

        public void SetMatches()
        {
            var players = Players.Where(p => p.IsChecked)
                .Select(p => p.PlayerItem)
                .ToList();
            Manager.CreateTournament(_tournament, players, AmountMatches, Manager.GetUserCredentials());
        }
        
        private async void LoadMatchesForTournamentAsync(Tournament tournament)
        {
            Matches.Clear();
            await Task.Run(() =>
            {
                var matches = Manager.GetAllMatchesFor(tournament);

                foreach (var match in matches)
                {
                    Matches.Add(new MatchVm(match));
                }
            });

        }
    }
}
