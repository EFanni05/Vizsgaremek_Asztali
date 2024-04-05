using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for MyProfileUpdate.xaml
    /// </summary>
    public partial class MyProfileUpdate : Page
    {
        public MyProfileUpdate()
        {
            InitializeComponent();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("MyPage.xaml", UriKind.Relative));
        }

        private int currentUserId = 0;

        private void OnUpdateClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO:test imput data
                var request = new UpdateUserResponse() 
                { 
                    Email = EmailTextBox.Text,
                    Id = currentUserId,
                    Name = UsernameTextBox.Text,
                };
                if (!string.IsNullOrEmpty(NewPasswordTextBox.Password))
                {
                    request.Password = NewPasswordTextBox.Password;
                    request.PasswordOld = CurrentPasswordTextBox.Password;
                    request.PasswordAgain = NewPasswordAgainTextBox.Password;
                }
                App.CurrentApp.APIClient.UpdateMe(request);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("MyPage.xaml", UriKind.Relative));
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var me = App.CurrentApp.APIClient.Me();
                if (me == null)
                {
                    throw new Exception("Login failed!");
                }
                else
                {
                    UsernameTextBox.Text = me.Name;
                    EmailTextBox.Text = me.Email;
                    currentUserId = me.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
