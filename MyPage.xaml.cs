using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vizsgaremek_Asztali
{
    /// <summary>
    /// Interaction logic for MyPage.xaml
    /// </summary>
    public partial class MyPage : Page
    {
        public MyPage()
        {
            InitializeComponent();
        }

        private void OnPageLoad(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            try
            {
                var me = App.CurrentApp.APIClient.Me();
                if (me == null)
                {
                    throw new Exception("Login failed!");
                }
                else
                {
                    UsernameLabel.Content = me.Name;
                    EmailLabel.Content = me.Email;
                    RoleLabel.Content = me.Role;
                }
            }
            catch (UnauthenticatedException)
            {
                MessageBox.Show("You're not authenticated!\nPlease login!");
                mainWindow.OnAuthenticatedChange(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnProfileEdit(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("MyProfileUpdate.xaml", UriKind.Relative));
        }
    }
}
