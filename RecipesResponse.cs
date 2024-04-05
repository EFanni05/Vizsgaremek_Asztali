using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    public class RecipesResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("pretime")]
        public int Preptime { get; set; }

        [JsonProperty("posted")]
        public string Posted {  get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
