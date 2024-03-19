using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    public class UpdateUserResponse : UsersResponse
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("passwordAgain")]
        public string PasswordAgain {  get; set; }

        [JsonProperty("passwordOld")]
        public string PasswordOld { get; set; }
    }
}
