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
    /// Interaction logic for RecipeUpdate.xaml
    /// </summary>
    public partial class RecipeUpdate : Page
    {
        public RecipeUpdate()
        {
            InitializeComponent();
        }

        private int id;

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
        }

        private void OnUpdateClickedRecipe(object sender, RoutedEventArgs e)
        {
            try
            {
                var request = new UpdateRecipes()
                {
                    Id = id,
                    Title = TitleTextBox.Text,
                    Content = ContentTextBox.Text,
                    Description = DescriptionTextBox.Text,
                };
                if (int.TryParse(PreptimeTextBox.Text, out int prep))
                {
                    request.Preptime = prep;
                    App.CurrentApp.APIClient.UpdateRecipe(request);
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
                }
                else
                {
                    throw new Exception("Preparation time isn't a number");
                }
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
            if (int.TryParse(s, out int recipeid))
            {
                id = recipeid;
                var recipe = App.CurrentApp.APIClient.SearchRecipe(recipeid);
                TitleTextBox.Text = recipe.Title;
                PreptimeTextBox.Text = recipe.Preptime.ToString();
                DescriptionTextBox.Text = recipe.Description;
                ContentTextBox.Text = recipe.Content;
            }
            else
            {
                MessageBox.Show("Something went wrong!");
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
            }
        }
    }
}
