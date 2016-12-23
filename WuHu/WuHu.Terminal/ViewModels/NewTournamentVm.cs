
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class NewTournamentVm : BaseVm
    {
        public IList<int> AmountVirtualization { get; } = new List<int>(Enumerable.Range(1, 100));
        private readonly Tournament _tournament;
        private int _amountMatches;
        
        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }

        public NewTournamentVm(Action showMatchList, Action reloadParent)
        {
            _tournament = new Tournament("", DateTime.Now);
            
            CancelCommand = new RelayCommand(_ => showMatchList?.Invoke());

            SubmitCommand = new RelayCommand(async _ =>
            {
                var players = Players.Where(p => p.IsChecked)
                    .Select(p => p.PlayerItem).ToList();
                _tournament.Datetime = DateTime.Now;
                showMatchList?.Invoke();
                await Task.Run(() => 
                    TournamentManager.CreateTournament(
                        _tournament, players, AmountMatches, AuthenticationManager.AuthenticatedCredentials));
                reloadParent?.Invoke();
            });

            LoadPlayersAsync();
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
    }
}
