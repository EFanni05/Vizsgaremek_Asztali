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
            UsersDataGrid.Items.Add(App.CurrentApp.APIClient.GetAllUsers());
        }

        private void ChangeRole(object sender, RoutedEventArgs e)
        {
            //TODO: uri change fo the frame
        }

        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            //TODO: uri change fo the frame
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            UsersResponse user = UsersDataGrid.SelectedItem;
            App.CurrentApp.APIClient.DeleteUser(user.Id);
        }

        private void SearchUser(object sender, RoutedEventArgs e)
        {
            string search = Searchbox.Text;
            try
            {
                List<UsersResponse> users = App.CurrentApp.APIClient.SearchUser(search);
                UsersDataGrid.Items.Add(users); //minus the id
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
