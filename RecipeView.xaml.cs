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
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Page
    {
        public RecipeView()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string uri = NavigationService.CurrentSource.ToString();
            string query = uri.Substring(uri.IndexOf('?'));
            NameValueCollection parameters = HttpUtility.ParseQueryString(query);
            string s = parameters.Get("userId") ?? "";
            if (int.TryParse(s, out int userid))
            {
                try
                {
                    var recipe = App.CurrentApp.APIClient.SearchRecipe(userid);
                    if (recipe == null)
                    {
                        MessageBox.Show("Something went wrong!");
                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
                    }
                    else
                    {
                        UsernameLabel.Content = recipe.Username;
                        TitleLabel.Content = recipe.Title;
                        Posted.Content = DateTime.Parse(recipe.Posted).ToString("yyyy-MM-dd HH:mm");
                        PrepTimeLabel.Content = recipe.Preptime.ToString() + " minutes";
                        DescriptionTextBlock.Text = recipe.Description;
                        ContentRecipes.Text = recipe.Content;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong! " + ex.Message);
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
                }
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
            }
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
        }
    }
}
