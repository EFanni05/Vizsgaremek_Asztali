using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    class Recipes
    {
        private int id;
        private string title;
        private string description;
        private string content;
        private int preptime;
        private string posted;
        private int user_id;

        public Recipes(int id, string title, string description, string content, int preptime, string posted, int user_id)
        {
            Id = id;
            Title = title;
            Description = description;
            Content = content;
            Preptime = preptime;
            Posted = posted;
            User_id = user_id;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string Content { get => content; set => content = value; }
        public int Preptime { get => preptime; set => preptime = value; }
        public string Posted { get => posted; set => posted = value; }
        public int User_id { get => user_id; set => user_id = value; }
    }
}
