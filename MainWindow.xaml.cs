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
            OnAuthenticatedChange(false);
        }

        public void OnAuthenticatedChange(bool authenticated)
        {
            var visibility = authenticated ? Visibility.Visible: Visibility.Hidden;
            HomeButton.Visibility = visibility;
            RecipesButton.Visibility = visibility;
            RatingsButton.Visibility = visibility;
            UsersButton.Visibility = visibility;
            AboutUsButton.Visibility = visibility;
            LogoutButton.Visibility = visibility;
            if (authenticated) 
            {
                FrameForPages.Navigate(new Uri("MyPage.xaml", UriKind.Relative));
            }
            else 
            {
                FrameForPages.Navigate(new Uri("Login.xaml", UriKind.Relative));
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.APIClient.LogOut();
            OnAuthenticatedChange(false);
        }

        private void MyPage(object sender, RoutedEventArgs e)
        {
            FrameForPages.Navigate(new Uri("MyPage.xaml", UriKind.Relative));
        }

        private void Recipes(object sender, RoutedEventArgs e)
        {
            FrameForPages.Navigate(new Uri("Recipes.xaml", UriKind.Relative));
        }

        private void Ratings(object sender, RoutedEventArgs e)
        {
            FrameForPages.Navigate(new Uri("Ratings.xaml", UriKind.Relative));
        }

        private void Users(object sender, RoutedEventArgs e)
        {
            FrameForPages.Navigate(new Uri("Users.xaml", UriKind.Relative));
        }

        private void AboutPopup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by Elekes Fanni 14.S");
        }
    }
}