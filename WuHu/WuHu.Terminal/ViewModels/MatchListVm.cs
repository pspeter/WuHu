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

        public ICommand ShowAddTournamentCommand { get; }

        public MatchListVm(Action<object> showAddTournament)
        {
            ShowAddTournamentCommand = new RelayCommand(
                showAddTournament,
                _ => IsAuthenticated);
            Matches = new ObservableCollection<MatchVm>();
            _tournament = Manager.GetMostRecentTournament();
            LoadMatchesForTournamentAsync(_tournament);
            _onMatchesLoaded += () =>
                CurrentMatch = Matches.Count > 0 ? Matches.First() : null;
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
            Matches.Clear();
            var matchVms = await Task.Run(() =>
                Manager.GetAllMatchesFor(tournament)
                .Select(m => new MatchVm(m)).ToList());

            foreach (var match in matchVms)
            {
                Matches.Add(match);
            }
            _onMatchesLoaded?.Invoke();

        }

        public override void Reload()
        {
            _tournament = Manager.GetMostRecentTournament();
            OnPropertyChanged(this, nameof(Name));
            LoadMatchesForTournamentAsync(_tournament);
        }
    }
}
