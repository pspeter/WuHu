using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.BL.Impl;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    class LoginVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isAuthenticated;
        private string _username;
        private string _password;
        private Authenticator _authenticator;

        public LoginVm(Authenticator authenticator)
        {
            _authenticator = authenticator;
            LoginCommand = new RelayCommand(c => _authenticator.Authenticate((Credentials) c, false));
        }


        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            set
            {
                if (value != _isAuthenticated)
                {
                    _isAuthenticated = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; private set; }
        

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
