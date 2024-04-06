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
    /// Interaction logic for Ratings.xaml
    /// </summary>
    public partial class Ratings : Page
    {
        public Ratings()
        {
            InitializeComponent();
        }

        public class DataItem
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Content { get; set; }
            public string Posted {  get; set; }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var rating = App.CurrentApp.APIClient.GetAllRating();
                if (rating == null || rating.Count == 0)
                {
                    throw new Exception("something gone wrong!");
                }
                else
                {
                    RatingDataGrid.ItemsSource = rating.Select(r => new DataItem
                    {
                        Id = r.Id,
                        Username = r.Username,
                        Content = r.Content,
                        Rating = r.Rating,
                        Posted = DateTime.Parse(r.Posted).ToString("yyyy-MM-dd HH:mm")
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var dataitem = source.DataContext as DataItem;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri($"UpdateRating.xaml?userId={dataitem.Id}", UriKind.Relative));
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var rating = source.DataContext as DataItem;
                if (rating != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            App.CurrentApp.APIClient.DeleteRating(rating.Id);
                            var deleteRating = RatingDataGrid.ItemsSource;
                            RatingDataGrid.ItemsSource = deleteRating.Cast<DataItem>().Where(u => u.Id != rating.Id);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to delete recipe: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
