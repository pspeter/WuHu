using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WuHu.Terminal.Views.Login;

namespace WuHu.Terminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            
            var dialog = new LoginWindow();

            if (dialog.ShowDialog() == true)
            {
                Console.WriteLine("yay");
            }
            else
            {
                //MessageBox.Show("Login failed", "Error", MessageBoxButton.OK);

                new Terminal().ShowDialog();
                Current.Shutdown(-1);
            }
        }
    }
}
