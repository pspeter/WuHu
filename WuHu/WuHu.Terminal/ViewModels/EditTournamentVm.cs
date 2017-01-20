using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.Services;

namespace WuHu.Terminal.ViewModels
{
    public class EditTournamentVm : BaseVm
    {
        public IList<int> AmountVirtualization { get; } = new List<int>(Enumerable.Range(1, 100));
        private readonly Tournament _tournament;
        private int _amountMatches;

        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }

        public EditTournamentVm(Tournament tournament, Action showMatchList, Action reloadParent, Action<string> queueMessage)
        {
            _tournament = tournament;
            var locked = AuthenticationService.IsAuthenticated() && 
                TournamentManager.LockTournament();
            if (!locked)
            {
                queueMessage?.Invoke("Spielplan wird zur Zeit bearbeitet. Bitte warten.");
                showMatchList?.Invoke();
            }

            CancelCommand = new RelayCommand(_ => showMatchList?.Invoke());

            SubmitCommand = new RelayCommand(async _ =>
            {
                var players = Players.Where(p => p.IsChecked)
                    .Select(p => p.PlayerItem).ToList();
                _tournament.Datetime = DateTime.Now;
                showMatchList?.Invoke();
                var success = await Task.Run(() =>
                {
                    var updated = AuthenticationService.IsAuthenticated() && TournamentManager.UpdateTournament(
                        _tournament, players, AmountMatches);
                    if (AuthenticationService.IsAuthenticated())
                    {
                        TournamentManager.UnlockTournament();
                    }
                    return updated;
                });
                reloadParent?.Invoke();
                queueMessage?.Invoke(success ? "Spielplan wurde geändert." : "Fehler: Spielplan konnte nicht geändert werden.");
            });

            LoadPlayersAsync();
        }

        public string Name => _tournament.Name;

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
