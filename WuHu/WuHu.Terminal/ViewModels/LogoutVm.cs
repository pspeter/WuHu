using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Terminal.Services;

namespace WuHu.Terminal.ViewModels
{
    public class LogoutVm : BaseVm
    {
        public ICommand LogoutCommand { get; private set; }
        public LogoutVm(Action<string> queueMessage)
        {
            LogoutCommand = new RelayCommand(_ =>
            {
                AuthenticationService.Logout();
                OnAuthenticatedChanged(this);
                queueMessage?.Invoke("Logged out.");
            });
        }
    }
}
