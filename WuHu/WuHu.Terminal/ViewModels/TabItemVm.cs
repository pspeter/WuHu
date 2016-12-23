using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Brushes = System.Drawing.Brushes;

namespace WuHu.Terminal.ViewModels
{
    public class TabItemVm: INotifyPropertyChanged
    {
        private string _header;
        private string _iconName;
        private bool _isEnabled;

        public event PropertyChangedEventHandler PropertyChanged;
        public BaseVm Content { get; }

        public TabItemVm(string header, string iconname, BaseVm content)
        {
            Header = header;
            IconName = iconname;
            Content = content;
            IsEnabled = true;
        }

        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                if (_header == value) return;
                _header = value;
                OnPropertyChanged();
            }
        }

        public string IconName {
            get
            {
                return _iconName;
            }
            set
            {
                if (_iconName == value) return;
                _iconName = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Background));
            }
        }

        public SolidColorBrush Background => IsEnabled ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.DimGray);

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
