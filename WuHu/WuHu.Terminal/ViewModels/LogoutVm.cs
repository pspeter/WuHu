using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
    public class LogoutVm : BaseVm
    {
        public ICommand LogoutCommand { get; private set; }
        public LogoutVm(Action<string> queueMessage)
        {
            LogoutCommand = new RelayCommand(_ =>
            {
                AuthenticationManager.Logout();
                OnAuthenticatedChanged(this);
                queueMessage?.Invoke("Logged out.");
            });
        }
    }
}
