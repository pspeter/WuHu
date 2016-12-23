using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class NewPlayerVm : BaseVm
    {
        public ICommand CancelCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand UploadCommand { get; }

        public NewPlayerVm(Action showPlayerList, Action reloadParent)
        {
            PlayerItem = new Player("", "", "", "", "", false,
                false, false, false, false, false, false, false, null);


            CancelCommand = new RelayCommand(_ => showPlayerList?.Invoke());

            SubmitCommand = new RelayCommand(async param =>
            {
                var pwBox = param as PasswordBox;
                // don't save password in memory, just send it to the Manager right away
                if (pwBox == null) return;

                var salt = CryptoService.GenerateSalt();
                var hash = CryptoService.HashPassword(pwBox.Password, salt);
                pwBox.Password = null;

                PlayerItem.Salt = salt;
                PlayerItem.Password = hash;
                showPlayerList?.Invoke();
                await Task.Run(() =>
                    PlayerManager.AddPlayer(PlayerItem, AuthenticationManager.AuthenticatedCredentials));
                reloadParent?.Invoke();
            });

            UploadCommand = new RelayCommand(o =>
                {
                    OpenFileDialog op = new OpenFileDialog
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

        public Player PlayerItem { get; }

        public string Firstname
        {
            get { return PlayerItem.Firstname; }
            set
            {
                if (PlayerItem.Firstname != value)
                {
                    PlayerItem.Firstname = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public string Nickname
        {
            get { return PlayerItem.Nickname; }
            set
            {
                if (PlayerItem.Nickname != value)
                {
                    PlayerItem.Nickname = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public string Lastname
        {
            get { return PlayerItem.Lastname; }
            set
            {
                if (PlayerItem.Lastname != value)
                {
                    PlayerItem.Lastname = value;
                    OnPropertyChanged(this);
                }
            }
        }
        public string Username
        {
            get { return PlayerItem.Username; }
            set
            {
                if (PlayerItem.Username != value)
                {
                    PlayerItem.Username = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool IsAdmin
        {
            get { return PlayerItem.IsAdmin; }
            set
            {
                if (PlayerItem.IsAdmin != value)
                {
                    PlayerItem.IsAdmin = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysMondays
        {
            get { return PlayerItem.PlaysMondays; }
            set
            {
                if (PlayerItem.PlaysMondays != value)
                {
                    PlayerItem.PlaysMondays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysTuesdays
        {
            get { return PlayerItem.PlaysTuesdays; }
            set
            {
                if (PlayerItem.PlaysTuesdays != value)
                {
                    PlayerItem.PlaysTuesdays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysWednesdays
        {
            get { return PlayerItem.PlaysWednesdays; }
            set
            {
                if (PlayerItem.PlaysWednesdays != value)
                {
                    PlayerItem.PlaysWednesdays = value;
                    OnPropertyChanged(this);
                }
            }
        }
        public bool PlaysThursdays
        {
            get { return PlayerItem.PlaysThursdays; }
            set
            {
                if (PlayerItem.PlaysThursdays != value)
                {
                    PlayerItem.PlaysThursdays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysFridays
        {
            get { return PlayerItem.PlaysFridays; }
            set
            {
                if (PlayerItem.PlaysFridays != value)
                {
                    PlayerItem.PlaysFridays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysSaturdays
        {
            get { return PlayerItem.PlaysSaturdays; }
            set
            {
                if (PlayerItem.PlaysSaturdays != value)
                {
                    PlayerItem.PlaysSaturdays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public bool PlaysSundays
        {
            get { return PlayerItem.PlaysSundays; }
            set
            {
                if (PlayerItem.PlaysSundays != value)
                {
                    PlayerItem.PlaysSundays = value;
                    OnPropertyChanged(this);
                }
            }
        }

        public BitmapImage Image
        {
            get { return LoadImage(PlayerItem.Picture); }
            set
            {
                var img = SaveImage(value);
                if (PlayerItem.Picture != img)
                {
                    PlayerItem.Picture = img;
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
    }
}
