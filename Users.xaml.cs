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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
        }

        public class DataItem
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void OnView(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var dataitem = source.DataContext as DataItem;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri($"UserView.xaml?userId={dataitem.Id}", UriKind.Relative));
            }
        }

        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var dataitem = source.DataContext as DataItem;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri($"UpdateUser.xaml?userId={dataitem.Id}", UriKind.Relative));
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var user = source.DataContext as DataItem;
                if (user != null) 
                {
                    var result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            App.CurrentApp.APIClient.DeleteUser(user.Id);
                            var users = UsersDataGrid.ItemsSource;
                            UsersDataGrid.ItemsSource = users.Cast<DataItem>().Where(u => u.Id != user.Id);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to delete user: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Searchbox.Text))
            {
                LoadUsers();
                return;
            }
            try
            {
                var users = App.CurrentApp.APIClient.SearchUser(Searchbox.Text);
                if (users == null)
                {
                    throw new Exception("something gone wrong!");
                }
                UsersDataGrid.ItemsSource = users.Select(r => new DataItem
                {
                    Id = r.Id,
                    Username = r.Name,
                    Email = r.Email,
                    Role = r.Role
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = App.CurrentApp.APIClient.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    throw new Exception("something gone wrong!");
                }
                UsersDataGrid.ItemsSource = users.Select(r => new DataItem
                {
                    Id = r.Id,
                    Username = r.Name,
                    Email = r.Email,
                    Role = r.Role
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
