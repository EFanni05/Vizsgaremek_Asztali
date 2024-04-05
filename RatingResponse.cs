using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    public class RatingResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("posted")]
        public string Posted { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("recipe_id")]
        public int RecipeId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
