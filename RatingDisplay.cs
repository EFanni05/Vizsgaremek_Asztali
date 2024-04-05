using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    public class RatingDisplay
    {
        public RatingDisplay(int id, string content, string posted, int rating, int recipeId, string user)
        {
            Id = id;
            Content = content;
            Posted = posted;
            Rating = rating;
            RecipeId = recipeId;
            User = user;
        }

        public int Id { get; set; }
        public string Content { get; set; }

        public string Posted { get; set; }

        public int Rating { get; set; }

        public int RecipeId { get; set; }

        public string User { get; set; }
    }
}
