using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.APIClient.LogOut();
        }

        private void MyPage(object sender, RoutedEventArgs e)
        {
            FrameForPages.Source = new Uri("MyPage.xaml");
        }

        private void Recipes(object sender, RoutedEventArgs e)
        {
            FrameForPages.Source = new Uri("Recipes.xaml");
        }

        private void Ratings(object sender, RoutedEventArgs e)
        {
            FrameForPages.Source = new Uri("Rating.xaml");
        }

        private void Users(object sender, RoutedEventArgs e)
        {
            FrameForPages.Source = new Uri("Users.xaml");
        }

        private void AboutPopup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by Elekes Fanni 14.S");
        }
    }
}