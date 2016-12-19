using System.Windows.Input;
using WuHu.BL.Impl;

namespace WuHu.Terminal.ViewModels
{
    internal class LoginVm : BaseVm
    {
        private string _username;
        private string _password;

        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public LoginVm()
        {
            LoginCommand = new RelayCommand(p => Manager.Login(Username, Password));
            LogoutCommand = new RelayCommand(p => Manager.Logout(), p => Manager.IsUserAuthenticated());
        }
        
        public string Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged(this);
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
                    OnPropertyChanged(this);
                }
            }
        }


        private void Login(object param)
        {
            var success = Manager.Login(Username, Password);

            if (success)
            {
                IsAuthenticatedChanged();
            }
        }

        private void Logout(object param)
        {
            Manager.Logout();
            IsAuthenticatedChanged();
        }
    }
}
