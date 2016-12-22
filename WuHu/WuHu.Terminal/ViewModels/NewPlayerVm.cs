using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class NewPlayerVm : BaseVm
    {
        private readonly Player _player;
        private string _password; // for creating a new Player
        
        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }

        public NewPlayerVm(Action showPlayerList, Action reloadParent)
        {
            _player = new Player("", "", "", "", "", false,
                false, false, false, false, false, false, false, null);


            CancelCommand = new RelayCommand(_ => showPlayerList?.Invoke());

            SubmitCommand = new RelayCommand(async _ =>
            {
                showPlayerList?.Invoke();
                await Task.Run(() =>
                    Manager.AddPlayer(_player, Manager.AuthenticatedCredentials));
                reloadParent?.Invoke();
            });
        }

        public Player PlayerItem => _player;

        public string Firstname
        {
            get { return _player.Firstname; }
            set
            {
                if (_player.Firstname != value)
                {
                    _player.Firstname = value;
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
                    OnPropertyChanged(this);
                }
            }
        }

        public string Passwort
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged(this);
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

        private bool ChangePassword(string newPassword)
        {
            return Manager.ChangePassword(
                _player.Username, newPassword, Manager.AuthenticatedCredentials);
        }
    }
}
