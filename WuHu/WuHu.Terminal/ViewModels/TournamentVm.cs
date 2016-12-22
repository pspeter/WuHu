using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private BaseVm _currentVm;

        private readonly NewTournamentVm _newTournamentVm;
        private readonly MatchListVm _matchListVm;
        
        public TournamentVm()
        {
            _tournament = Manager.GetMostRecentTournament();

            _matchListVm = new MatchListVm(p => SwitchVm(_newTournamentVm));
            _newTournamentVm = new NewTournamentVm(
                () => SwitchVm(_matchListVm),
                () =>
                {
                    _matchListVm.Reload();
                });

            CurrentVm = _matchListVm;
        }

        public BaseVm CurrentVm
        {
            get { return _currentVm; }
            set
            {
                if (!Equals(_currentVm, value))
                {
                    _currentVm = value;
                    OnPropertyChanged(this);
                }
            }
        }

        private void SwitchVm(BaseVm vm)
        {
            CurrentVm = vm;
        }

        public ICommand SetMatchesCommand { get; }
    }
}
