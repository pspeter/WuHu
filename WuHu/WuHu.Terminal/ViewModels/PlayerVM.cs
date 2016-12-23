using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerVm : BaseVm
    {
        private Player _player;
        private Player _oldPlayer;
        private Rating _currentRating;
        private bool _isChecked;  // for creating a new Tournament
        private bool _isDirty; 

        public PlayerVm(Action reloadParent)
        {
            _player = new Player("", "", "", "", "", false, 
                false, false, false, false, false, false, false, null);
            _currentRating = null;
            SetChecked();
        }

        public ICommand SavePlayerCommand { get; }
        public ICommand RevertChangesCommand { get; }
        public ICommand UploadCommand { get; }

        public Player PlayerItem => _player;

        public PlayerVm(Player player, Action<string> reloadParent)
        {
            _player = player;
            _oldPlayer = new Player(_player.PlayerId ?? -1, _player.Firstname, _player.Lastname,
                        _player.Nickname, _player.Username,
                        _player.Password, _player.Salt, _player.IsAdmin, _player.PlaysMondays, _player.PlaysTuesdays,
                        _player.PlaysWednesdays, _player.PlaysThursdays, _player.PlaysFridays, _player.PlaysSaturdays, 
                        _player.PlaysSundays, _player.Picture);
            _currentRating = RatingManager.GetCurrentRatingFor(player);

            SetChecked();

            IsDirty = false;
            
            SavePlayerCommand = new RelayCommand(async _ =>
                {
                    IsDirty = false;
                    _oldPlayer = new Player(_player.PlayerId ?? -1, _player.Firstname, _player.Lastname,
                        _player.Nickname, _player.Username,
                        _player.Password, _player.Salt, _player.IsAdmin, _player.PlaysMondays, _player.PlaysTuesdays,
                        _player.PlaysWednesdays,
                        _player.PlaysThursdays, _player.PlaysFridays, _player.PlaysSaturdays, _player.PlaysSundays,
                        _player.Picture);
                    var success = await Task.Run(() =>
                        PlayerManager.UpdatePlayer(_player, AuthenticationManager.AuthenticatedCredentials));
                    reloadParent?.Invoke(success ? "Spieler geändert." : "Fehler: Spieler konnte nicht geändert werden.");
                },
                _ => IsDirty && IsAuthenticated
            );

            RevertChangesCommand = new RelayCommand(_ =>
                {
                
                    IsDirty = false;
                    _player = _oldPlayer;
                    _oldPlayer = new Player(_player.PlayerId ?? -1, _player.Firstname, _player.Lastname, _player.Nickname, _player.Username,
                        _player.Password, _player.Salt, _player.IsAdmin, _player.PlaysMondays, _player.PlaysTuesdays, _player.PlaysWednesdays,
                        _player.PlaysThursdays, _player.PlaysFridays, _player.PlaysSaturdays, _player.PlaysSundays, _player.Picture);
                    OnPropertyChanged(this);
                    OnPropertyChanged(this, nameof(IsChecked));
                    OnPropertyChanged(this, nameof(Firstname));
                    OnPropertyChanged(this, nameof(Nickname));
                    OnPropertyChanged(this, nameof(Lastname));
                    OnPropertyChanged(this, nameof(Username));
                    OnPropertyChanged(this, nameof(IsAdmin));
                    OnPropertyChanged(this, nameof(PlaysMondays));
                    OnPropertyChanged(this, nameof(PlaysTuesdays));
                    OnPropertyChanged(this, nameof(PlaysWednesdays));
                    OnPropertyChanged(this, nameof(PlaysThursdays));
                    OnPropertyChanged(this, nameof(PlaysFridays));
                    OnPropertyChanged(this, nameof(PlaysSaturdays));
                    OnPropertyChanged(this, nameof(PlaysSundays));
                    OnPropertyChanged(this, nameof(Image));
                },
                _ => IsDirty && IsAuthenticated
            );

            UploadCommand = new RelayCommand(o =>
            {
                var op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg|" +
                             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg)"
                };
                if (op.ShowDialog() == true)
                {
                    Image = new BitmapImage(new Uri(op.FileName));
                }
            }
            );
        }

        public bool IsDirty
        {
            get { return _isDirty; } 
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                OnPropertyChanged(this);
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                IsDirty = true;
                OnPropertyChanged(this);
            }
        }

        public string Firstname
        {
            get { return _player.Firstname; }
            set
            {
                if (_player.Firstname != value)
                {
                    _player.Firstname = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public string Nickname
        {
            get { return _player.Nickname; }
            set
            {
                if (_player.Nickname != value)
                {
                    _player.Nickname = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public string Lastname
        {
            get { return _player.Lastname; }
            set
            {
                if (_player.Lastname != value)
                {
                    _player.Lastname = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

         public string Username
        {
            get { return _player.Username; }
            set
            {
                if (_player.Username != value)
                {
                    _player.Username = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool IsAdmin
        {
            get { return _player.IsAdmin; }
            set
            {
                if (_player.IsAdmin != value)
                {
                    _player.IsAdmin = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysMondays
        {
            get { return _player.PlaysMondays; }
            set
            {
                if (_player.PlaysMondays != value)
                {
                    _player.PlaysMondays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysTuesdays
        {
            get { return _player.PlaysTuesdays; }
            set
            {
                if (_player.PlaysTuesdays != value)
                {
                    _player.PlaysTuesdays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysWednesdays
        {
            get { return _player.PlaysWednesdays; }
            set
            {
                if (_player.PlaysWednesdays != value)
                {
                    _player.PlaysWednesdays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }
        public bool PlaysThursdays
        {
            get { return _player.PlaysThursdays; }
            set
            {
                if (_player.PlaysThursdays != value)
                {
                    _player.PlaysThursdays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysFridays
        {
            get { return _player.PlaysFridays; }
            set
            {
                if (_player.PlaysFridays != value)
                {
                    _player.PlaysFridays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysSaturdays
        {
            get { return _player.PlaysSaturdays; }
            set
            {
                if (_player.PlaysSaturdays != value)
                {
                    _player.PlaysSaturdays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysSundays
        {
            get { return _player.PlaysSundays; }
            set
            {
                if (_player.PlaysSundays != value)
                {
                    _player.PlaysSundays = value;
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public Rating CurrentRating
        {
            get { return _currentRating; }
            set
            {
                if (_currentRating != value)
                {
                    _currentRating = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public BitmapImage Image
        {
            get { return LoadImage(_player.Picture); }
            set
            {
                var img = SaveImage(value);
                if (_player.Picture != SaveImage(value))
                {
                    _player.Picture = SaveImage(value);
                    IsDirty = true;
                    OnPropertyChanged(this);
                }
            }
        }

        public string ExtendedFullname => Firstname + " '" + Nickname + "' " + Lastname;
        public string Fullname => Firstname + " " + Lastname;

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private byte[] SaveImage(BitmapImage image)
        {
            var memStream = new MemoryStream();
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void SetChecked()
        {
            var today = DateTime.Now.DayOfWeek;
            switch (today)
            {
                case DayOfWeek.Monday:
                    {
                        IsChecked = _player.PlaysMondays;
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        IsChecked = _player.PlaysTuesdays;
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        IsChecked = _player.PlaysWednesdays;
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        IsChecked = _player.PlaysThursdays;
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        IsChecked = _player.PlaysFridays;
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        IsChecked = _player.PlaysSaturdays;
                        break;
                    }
                case DayOfWeek.Sunday:
                    {
                        IsChecked = _player.PlaysSundays;
                        break;
                    }
                default:
                    {
                        IsChecked = false;
                        break;
                    }
            }
        }
    }
}
