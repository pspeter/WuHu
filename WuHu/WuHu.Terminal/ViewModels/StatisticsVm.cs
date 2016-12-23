using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WuHu.Terminal.ViewModels
{
    public class StatisticsVm : BaseVm
    {
        public class PlayerRatingEntry
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Rating { get; set; }
        }

        private PlayerVm _playerVm;


        public StatisticsVm()
        {
            LoadPlayersAsync();
            RefreshCommand = new RelayCommand(_ =>
            {
                OnPropertyChanged(this, nameof(Player1Entries));
                OnPropertyChanged(this, nameof(Player2Entries));
                OnPropertyChanged(this, nameof(Player3Entries));
                OnPropertyChanged(this, nameof(Player4Entries));
                OnPropertyChanged(this, nameof(Player1Name));
                OnPropertyChanged(this, nameof(Player2Name));
                OnPropertyChanged(this, nameof(Player3Name));
                OnPropertyChanged(this, nameof(Player4Name));
            });
            OnPlayersLoaded += () => RefreshCommand.Execute(null);
        }

        public PlayerVm SelectedPlayer
        {
            get { return _playerVm; }
            set
            {
                if (_playerVm == value) return;
                _playerVm = value;
                OnPropertyChanged(this);
            }
        }

        public ICommand RefreshCommand { get; set; }

        public string Player1Name => Player1Entries.First().Name;
        public string Player2Name => Player2Entries.First().Name;
        public string Player3Name => Player3Entries.First().Name;
        public string Player4Name => Player4Entries.First().Name;

        public IEnumerable<PlayerRatingEntry> Player1Entries
        {
            get
            {
                var entries = new List<PlayerRatingEntry>();
                var player = Players
                    .Where(p => p.IsChecked)
                    .Skip(0)
                    .First();

                    RatingManager.GetAllRatingsFor(player.PlayerItem)
                        .OrderByDescending(r => r.Datetime)
                        .ToList()
                        .ForEach(rating => entries.Add(new PlayerRatingEntry
                        {
                            Date = rating.Datetime,
                            Name = player.Fullname,
                            Rating = rating.Value
                        }));
                return entries;
            }
        }

        public IEnumerable<PlayerRatingEntry> Player2Entries
        {
            get
            {
                var entries = new List<PlayerRatingEntry>();
                var player = Players
                    .Where(p => p.IsChecked)
                    .Skip(1)
                    .First();

                RatingManager.GetAllRatingsFor(player.PlayerItem)
                    .OrderByDescending(r => r.Datetime)
                    .ToList()
                    .ForEach(rating => entries.Add(new PlayerRatingEntry
                    {
                        Date = rating.Datetime,
                        Name = player.Fullname,
                        Rating = rating.Value
                    }));
                return entries;
            }
        }

        public IEnumerable<PlayerRatingEntry> Player3Entries
        {
            get
            {
                var entries = new List<PlayerRatingEntry>();
                var player = Players
                    .Where(p => p.IsChecked)
                    .Skip(2)
                    .First();

                var ratings = RatingManager.GetAllRatingsFor(player.PlayerItem)
                    .OrderByDescending(r => r.Datetime)
                    .ToList();


                ratings.ForEach(rating => entries.Add(new PlayerRatingEntry
                    {
                        Date = rating.Datetime,
                        Name = player.Fullname,
                        Rating = rating.Value
                    }));
                return entries;
            }
        }

        public IEnumerable<PlayerRatingEntry> Player4Entries
        {
            get
            {
                var entries = new List<PlayerRatingEntry>();
                var player = Players
                    .Where(p => p.IsChecked)
                    .Skip(3)
                    .First();

                RatingManager.GetAllRatingsFor(player.PlayerItem)
                    .OrderByDescending(r => r.Datetime)
                    .ToList()
                    .ForEach(rating => entries.Add(new PlayerRatingEntry
                    {
                        Date = rating.Datetime,
                        Name = player.Fullname,
                        Rating = rating.Value
                    }));
                return entries;
            }
        }
    }
}
