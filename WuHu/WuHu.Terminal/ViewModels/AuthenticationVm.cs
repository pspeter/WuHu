using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace WuHu.Terminal.ViewModels
{
    public class AuthenticationVm : BaseVm
    {
        private BaseVm _currentVm;

        private readonly LoginVm _loginVm;
        private readonly LogoutVm _logoutVm;

        public BaseVm CurrentVm
        {
            get { return _currentVm;}
            set
            {
                if (_currentVm == value) return;
                _currentVm = value;
                OnPropertyChanged(this);
            }
        }

        public AuthenticationVm(Action<string> queueMessage)
        {
            _loginVm = new LoginVm(msg =>
            {
                queueMessage?.Invoke(msg);
                SetVm();
            });
            _logoutVm = new LogoutVm(msg =>
            {
                queueMessage?.Invoke(msg);
                SetVm();
            });

            SetVm();
        }

        private void SetVm()
        {
            CurrentVm = IsAuthenticated ? _logoutVm as BaseVm : _loginVm;
        }
    }
}
