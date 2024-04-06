using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Page
    {
        public UserView()
        {
            InitializeComponent();
        }

        public class DataItem
        {
            public int Id { get; set; }
            public string Posted { get; set; }
            public string User { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string uri = NavigationService.CurrentSource.ToString();
            string query = uri.Substring(uri.IndexOf('?'));
            NameValueCollection parameters = HttpUtility.ParseQueryString(query);
            string s = parameters.Get("userId") ?? "";
            if (int.TryParse(s, out int userid)) 
            {
                GetRecipesAll(userid);
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
            }
        }

        private void GetRecipesAll(int userid)
        {
            try
            {
                var recipes = App.CurrentApp.APIClient.SearchUser(userid);
                if (recipes == null || recipes.Count == 0)
                {
                    MessageBox.Show("This user hasn't posted anything yet!");
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
                }
                else
                {
                    UsersDataGrid.ItemsSource = recipes.Select(r => new DataItem
                    {
                        Id = r.Id,
                        Posted = DateTime.Parse(r.Posted).ToString("yyyy-MM-dd HH:mm"),
                        User = r.Username,
                        Title = r.Title,
                        Description = r.Description
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
        }
    }
}
