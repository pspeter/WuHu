using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerPageVm : BaseVm
    {
        private BaseVm _currentVm;
        private readonly PlayerListVm _playerListVm;
        
        public PlayerPageVm(Action reloadTabs, Action<string> queueMessage)
        {
            _playerListVm = new PlayerListVm(
                o => SwitchVm(new NewPlayerVm(
                    () => SwitchVm(_playerListVm),
                    () => _playerListVm.Reload(),
                    queueMessage)
                ),
                reloadTabs,
                queueMessage
            );
            SwitchVm(_playerListVm);
        }

        public BaseVm CurrentVm
        {
            get { return _currentVm; }
            set
            {
                if (Equals(_currentVm, value)) return;
                _currentVm = value;
                OnPropertyChanged(this);
            }
        }

        private void SwitchVm(BaseVm vm)
        {
            CurrentVm = vm;
        }
    }
}
