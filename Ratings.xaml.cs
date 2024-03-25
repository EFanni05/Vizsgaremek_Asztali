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
            var response = App.CurrentApp.APIClient.GetAllRating();
            if (response != null )
            {
                List<RatingResponse> ratingsWithId = response;
                List<RatingDisplay> displayList = new List<RatingDisplay>();
                UsersResponse user;
                for ( int i = 0; i < ratingsWithId.Count; i++ )
                {
                    try
                    {
                        user = App.CurrentApp.APIClient.GetUser(ratingsWithId[i].UserId);
                        displayList.Add(new RatingDisplay(ratingsWithId[i].Id, ratingsWithId[i].Content, ratingsWithId[i].Posted, ratingsWithId[i].Rating, ratingsWithId[i].RecipeId, user.Name));
                    }catch ( Exception e )
                    {
                        Console.WriteLine( e.ToString() );
                    }
                }
                RatingDataGrid.Items.Add(displayList);
            }
        }
    }
}
