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
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
