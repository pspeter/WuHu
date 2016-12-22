using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Terminal.ViewModels
{
    public class TabItemVm: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BaseVm Content { get; }

        public TabItemVm(string header, string iconname, BaseVm content)
        {
            Header = header;
            IconName = iconname;
            Content = content;
        }

        public string Header { get; private set; }
        public string IconName { get; private set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
