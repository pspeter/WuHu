using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL;
using WuHu.BL.Impl;

namespace WuHu.Terminal.ViewModels
{
    public class BaseVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected ITerminalManager Manager;

        public bool IsAuthenticated => Manager.IsUserAuthenticated();
        public bool IsNotAuthenticated => !Manager.IsUserAuthenticated();
        public void IsAuthenticatedChanged()
        {
            OnPropertyChanged(IsAuthenticated);
            OnPropertyChanged(IsNotAuthenticated);
        }

        protected virtual void OnPropertyChanged(object context, [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(context, new PropertyChangedEventArgs(propertyName));
        }

        public BaseVm()
        {
            Manager = ManagerFactory.GetTerminalManager();
        }
    }
}
