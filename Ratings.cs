using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    class Ratings
    {
        private int id;
        private int rating;
        private string content;
        private string posted;
        private int user_id;
        private int recipe_id;

        public Ratings(int id, int rating, string content, string posted, int user_id, int recipe_id)
        {
            Id = id;
            Rating = rating;
            Content = content;
            Posted = posted;
            User_id = user_id;
            Recipe_id = recipe_id;
        }

        public int Id { get => id; set => id = value; }
        public int Rating { get => rating; set => rating = value; }
        public string Content { get => content; set => content = value; }
        public string Posted { get => posted; set => posted = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public int Recipe_id { get => recipe_id; set => recipe_id = value; }
    }
}
