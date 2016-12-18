using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WuHu.BL;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerVm : INotifyPropertyChanged
    {
        private IPlayerManager _playerManager;
        private Player _player;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PlayerVm(Player player, IPlayerManager playerManager)
        {
            _playerManager = playerManager;
            _player = player;
        }

        public string Firstname
        {
            get { return _player.Firstname; }
            set
            {
                if (_player.Firstname != value)
                {
                    _player.Firstname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Firstname)));
                }
            }
        }

        public string Nickname
        {
            get { return _player.Nickname; }
            set
            {
                if (_player.Nickname != value)
                {
                    _player.Nickname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nickname)));
                }
            }
        }

        public string Lastname
        {
            get { return _player.Lastname; }
            set
            {
                if (_player.Lastname != value)
                {
                    _player.Lastname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lastname)));
                }
            }
        }

        public ChangePassword()
    }
}
