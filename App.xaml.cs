using System.Configuration;
using System.Data;
using System.Windows;

namespace Vizsgaremek_Asztali
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public APIClient APIClient { get; set; }

        public static App CurrentApp => (App)Current;

        public App()
        {
            // TODO: get base address from app config
            APIClient = new APIClient("http://localhost:3000");
        }
    }
}
