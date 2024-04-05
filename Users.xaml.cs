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
            try
            {
                var users = App.CurrentApp.APIClient.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    throw new Exception("something gone wrong!");
                }
                else
                {
                    UsersDataGrid.ItemsSource = users.Select(r => new DataItem
                    {
                        Id = r.Id,
                        Username = r.Name,
                        Email = r.Email,
                        Role = r.Role
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeRole(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            
        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
