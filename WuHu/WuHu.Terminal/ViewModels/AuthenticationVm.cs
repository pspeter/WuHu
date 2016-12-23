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

        public AuthenticationVm(Action<string> notifyAuthenticationChanged)
        {
            _loginVm = new LoginVm(msg =>
            {
                notifyAuthenticationChanged?.Invoke(msg);
                SetVm();
            });
            _logoutVm = new LogoutVm(() =>
            {
                notifyAuthenticationChanged?.Invoke("Erfolgreich ausgeloggt.");
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
