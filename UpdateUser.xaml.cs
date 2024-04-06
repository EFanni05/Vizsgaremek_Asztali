using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// Interaction logic for UpdateUser.xaml
    /// </summary>
    public partial class UpdateUser : Page
    {
        public UpdateUser()
        {
            InitializeComponent();
        }

        private int id;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string uri = NavigationService.CurrentSource.ToString();
            string query = uri.Substring(uri.IndexOf('?'));
            NameValueCollection parameters = HttpUtility.ParseQueryString(query);
            string s = parameters.Get("userId") ?? "";
            if (int.TryParse(s, out int userid))
            {
                id = userid;
                GetUser(userid);
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
            }
        }

        private void GetUser(int userid)
        {
            try
            {
                var user = App.CurrentApp.APIClient.GetUser(userid);
                if (user == null)
                {
                    throw new Exception("Query failed");
                }
                else
                {
                    UsernameTextBox.Text = user.Name;
                    EmailTextBox.Text = user.Email;
                    string role = user.Role switch 
                    {
                        "user" => "User",
                        "admin" => "Admin",
                        "manager" => "Manager",
                        _ => "User"
                    };
                    var item = RoleDropDown.Items
                        .Cast<ComboBoxItem>()
                        .Where(x => x.Name == role)
                        .Single();
                    RoleDropDown.SelectedValue = item;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message );
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
            }
        }

        private void OnUpdateClickedUser(object sender, RoutedEventArgs e)
        {
            try
            {
                var roleItem = RoleDropDown.SelectedItem as ComboBoxItem;
                string role = (roleItem == null ? "User" : roleItem.Name) switch
                {
                    "User" => "user",
                    "Admin" => "admin",
                    "Manager" => "manager",
                    _ => "user",
                };
                var request = new UpdateUserResponse()
                {
                    Email = EmailTextBox.Text,
                    Id = id,
                    Name = UsernameTextBox.Text,
                    Role = role
                };
                if (!string.IsNullOrEmpty(NewPasswordTextBox.Password))
                {
                    request.Password = NewPasswordTextBox.Password;
                    request.PasswordAgain = NewPasswordAgainTextBox.Password;
                }
                App.CurrentApp.APIClient.UpdateAdminUser(request);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
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

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
        }
    }
}
