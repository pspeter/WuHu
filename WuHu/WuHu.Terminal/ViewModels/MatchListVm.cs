using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.ViewModels
{
    public class MatchListVm : BaseVm
    {
        private MatchVm _currentMatch;
        private Tournament _tournament;
        private readonly Action _onMatchesLoaded;
        private readonly Action _reloadTabs;

        public ICommand ShowAddTournamentCommand { get; }
        public ICommand ShowEditTournamentCommand { get; }

        public MatchListVm(Action<object> showAddTournament,
            Action<Tournament> showEditTournament,
            Action reloadTabs,
            Action<string> queueMessage)
        {
            _tournament = TournamentManager.GetMostRecentTournament();

            ShowAddTournamentCommand = new RelayCommand(
                showAddTournament,
                _ => IsAuthenticated);
            ShowEditTournamentCommand = new RelayCommand(o =>
                showEditTournament(_tournament),
                _ => IsAuthenticated);

            Matches = new ObservableCollection<MatchVm>();

            LoadMatchesForTournamentAsync(_tournament);
            _onMatchesLoaded += () =>
                CurrentMatch = Matches.Count > 0 ? Matches.First() : null;
            _reloadTabs = reloadTabs;
        }
        public MatchVm CurrentMatch
        {
            get { return _currentMatch; }
            set
            {
                if (!Equals(_currentMatch, value))
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

        public ObservableCollection<MatchVm> Matches { get; }

        private async void LoadMatchesForTournamentAsync(Tournament tournament)
        {
            var matchVms = await Task.Run(() =>
                MatchManager.GetAllMatchesFor(tournament)
                .Select(m => new MatchVm(m, Reload)).ToList());
            
            Matches.Clear();
            foreach (var match in matchVms)
            {
                Matches.Add(match);
            }
            _onMatchesLoaded?.Invoke();

        }

        public override void Reload()
        {
            _reloadTabs?.Invoke();
            _tournament = TournamentManager.GetMostRecentTournament();
            OnPropertyChanged(this, nameof(Name));
            LoadMatchesForTournamentAsync(_tournament);
        }
    }
}
