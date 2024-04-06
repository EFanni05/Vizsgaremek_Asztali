using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// Interaction logic for Recipes.xaml
    /// </summary>
    public partial class Recipes : Page
    {
        public Recipes()
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
            try
            {
                var recipes = App.CurrentApp.APIClient.GetAllRecipes();
                if(recipes == null || recipes.Count == 0)
                {
                    throw new Exception("something gone wrong!");
                }
                else
                {
                    RecipesDataGrid.ItemsSource = recipes.Select(r => new DataItem
                    {
                        Id = r.Id,
                        Posted = FormatDateTime(r.Posted),
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

        private string FormatDateTime(string timestamp)
        {
            return DateTime.Parse(timestamp).ToString("yyyy-MM-dd HH:mm");
        }

        private void OnViewClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var dataitem = source.DataContext as DataItem;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri($"RecipeView.xaml?userId={dataitem.Id}", UriKind.Relative));
            }
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var recipe = source.DataContext as DataItem;
                if (recipe != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            App.CurrentApp.APIClient.DeleteUser(recipe.Id);
                            var users = RecipesDataGrid.ItemsSource;
                            RecipesDataGrid.ItemsSource = users.Cast<DataItem>().Where(u => u.Id != recipe.Id);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to delete recipe: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button source)
            {
                var dataitem = source.DataContext as DataItem;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.FrameForPages.Navigate(new Uri($"RecipeUpdate.xaml?userId={dataitem.Id}", UriKind.Relative));
            }
        }
    }
}
