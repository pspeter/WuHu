using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class MatchVm : BaseVm
    {
        private Match _match;

        public MatchVm(Match match)
        {
            _match = match;
            ScoreVirtualization = new List<int>(Enumerable.Range(0, 100));
        }

        public Player Player1 => _match.Player1;
        public Player Player2 => _match.Player2;
        public Player Player3 => _match.Player3;
        public Player Player4 => _match.Player4;
        
        public IList<int> ScoreVirtualization { get; }

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
                }
            }
        }
    }
}
