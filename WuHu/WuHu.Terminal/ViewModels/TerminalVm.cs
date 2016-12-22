using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using WuHu.Terminal.Views;

namespace WuHu.Terminal.ViewModels
{
    public class TerminalVm : BaseVm
    {
        
        public ICommand LogoutCommand { get; private set; }

        public TerminalVm()
        {
            // commands
            LogoutCommand = new RelayCommand(
                Logout,
                p => IsAuthenticated);

        }
        

        private void Logout(object o)
        {
            Manager.Logout();
            OnAuthenticatedChanged(this);
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Manager.Logout();
            OnAuthenticatedChanged(this);
        }
    }
}
