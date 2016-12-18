using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
    public class PlayerVm : BaseVm
    {
        private Player _player;

        public PlayerVm(Player player = null)
        {
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
                    OnPropertyChanged(this);
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
                    OnPropertyChanged(this);
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
                    OnPropertyChanged(this);
                }
            }
        }

        private bool ChangePassword(string newPassword)
        {
            return Manager.ChangePassword(
                _player.Username, newPassword, Manager.GetUserCredentials());
        }
    }
}
