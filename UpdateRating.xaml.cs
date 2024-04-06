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
    /// Interaction logic for UpdateRating.xaml
    /// </summary>
    public partial class UpdateRating : Page
    {
        public UpdateRating()
        {
            InitializeComponent();
        }

        private int id;

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("Ratings.xaml", UriKind.Relative));
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var ratingItem = RatingDropDown.SelectedItem as ComboBoxItem;
                int rating = (ratingItem == null ? "One" : ratingItem.Name) switch
                {
                    "Five" => 5,
                    "Four" => 4,
                    "Three" => 3,
                    "Two" => 2,
                    "One" => 1,
                    _ => 1,
                };
                var request = new RatingUpdateResponse()
                {
                    Content = ContentTextBox.Text,
                    Rating = rating,
                };
                
                App.CurrentApp.APIClient.UpdateRating(request);
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Ratings.xaml", UriKind.Relative));
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
            string uri = NavigationService.CurrentSource.ToString();
            string query = uri.Substring(uri.IndexOf('?'));
            NameValueCollection parameters = HttpUtility.ParseQueryString(query);
            string s = parameters.Get("userId") ?? "";
            if (int.TryParse(s, out int ratingid))
            {
                id = ratingid;
                var rating = App.CurrentApp.APIClient.GetRating(ratingid);
                ContentTextBox.Text = rating.Content;
                if (rating == null)
                {
                    throw new Exception("Query failed");
                }
                else
                {
                    string role = rating.Rating switch
                    {
                        1 => "One",
                        2 => "Two",
                        3 => "Three",
                        4 => "Four",
                        5 => "Five",
                        _ => "One"
                    };
                    var item = RatingDropDown.Items
                        .Cast<ComboBoxItem>()
                        .Where(x => x.Name == role)
                        .Single();
                    RatingDropDown.SelectedValue = item;
                }
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Ratings.xaml", UriKind.Relative));
            }
        }
    }
}
