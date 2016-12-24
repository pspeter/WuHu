using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class MatchVm : BaseVm
    {
        private readonly Match _match;

        public ICommand SetScoreCommand { get; }

        public MatchVm(Match match, Action reloadParent, Action<string> queueMessage)
        {
            _match = match;

            SetScoreCommand = new RelayCommand(async _ =>
                {
                    IsDone = true;
                    var success = await Task.Run(() => MatchManager.SetScore(_match, AuthenticationManager.AuthenticatedCredentials));
                    queueMessage?.Invoke(success ? "Neue Wertungen berechnet." : "Fehler: Spielresultat konnte nicht gesetzt werden.");
                    reloadParent?.Invoke();
                }
                ,
            o => ScoreTeam1 != null && ScoreTeam2 != null &&
                 ScoreTeam1 >= 0    && ScoreTeam2 >= 0
            );
        }

        public Player Player1 => _match.Player1;
        public Player Player2 => _match.Player2;
        public Player Player3 => _match.Player3;
        public Player Player4 => _match.Player4;
        
        public IList<int> ScoreVirtualization { get; } = new List<int>(Enumerable.Range(0, 100));

        public DateTime Datetime
        {
            get { return _match.Datetime; }
            set
            {
                if (_match.Datetime != value)
                {
                    _match.Datetime = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public byte? ScoreTeam1
        {
            get { return _match.ScoreTeam1; }
            set
            {
                if (_match.ScoreTeam1 != value)
                {
                    _match.ScoreTeam1 = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public byte? ScoreTeam2
        {
            get { return _match.ScoreTeam2; }
            set
            {
                if (_match.ScoreTeam2 != value)
                {
                    _match.ScoreTeam2 = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public double EstimatedWinChance
        {
            get { return _match.EstimatedWinChance; }
            set
            {
                // comparison of floating point with tolerance
                // for floating point inaccuracies
                if (Math.Abs(_match.EstimatedWinChance - value) > 0.001)
                {
                    _match.EstimatedWinChance = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool IsDone
        {
            get { return _match.IsDone; }
            set
            {
                if (_match.IsDone != value)
                {
                    _match.IsDone = value;
                    OnPropertyChanged(this);
                    OnPropertyChanged(this, nameof(CanEdit));
                }
            }
        }

        public bool CanEdit => !IsDone && IsAuthenticated;
    }
}
