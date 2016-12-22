using System.Windows.Controls;
using WuHu.Terminal.ViewModels;

namespace WuHu.Terminal.Views
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LoginVm();
        }
    }
}
