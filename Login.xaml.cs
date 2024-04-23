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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;

namespace Vizsgaremek_Asztali
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly Regex regexEmail = new Regex(@"^[a-zA-Z0-9.!#$%&’*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", RegexOptions.Compiled);
        private readonly Regex regexPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", RegexOptions.Compiled);

        public Login()
        {
            InitializeComponent();
        }

        private void LoginAdmin(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            try
            {
                string email = Email_login.Text;
                string password = Password_Login.Password;
                if (email == null || password == null)
                {
                    throw new Exception("Email or Password is emty!");
                }
                if (!regexEmail.IsMatch(email))
                {
                    throw new Exception("Email format is invalid");
                }
                if (!regexPassword.IsMatch(password)) 
                {
                    throw new Exception("Password is not vaild");
                }
                App.CurrentApp.APIClient.Login(email, password);
                mainWindow.OnAuthenticatedChange(true);
            }
            catch (Exception ex)
            {
                mainWindow.OnAuthenticatedChange(false);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
