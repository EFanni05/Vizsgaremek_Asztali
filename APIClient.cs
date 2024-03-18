using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek_Asztali
{
    /// <summary>
    /// API client implementation
    /// </summary>
    public class APIClient
    {
        class LoginRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }

            public LoginRequest(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }
        class UserFunctionWithId
        {

            [JsonProperty("id")]
            public int Id { get; set; }

            public UserFunctionWithId(int id)
            {
                Id = id;
            }
        }

        class LoginResponse
        {
            [JsonProperty("token")]
            public string? AuthToken { get; set; }
        }

        private static HttpClient client = new HttpClient();
        
        public APIClient(string baseAddress) 
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }


        public void Login(string email, string password)
        {
            LogOut();
            var body = JsonContent.Create<LoginRequest>(new LoginRequest(email, password));
            var response = client.PostAsync("/auth/login", body).Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<LoginResponse>(json);
            if (result?.AuthToken == null)
            {
                throw new ApplicationException("API error (invalid response or token is received)");
            }
            Properties.Settings.Default.AuthToken = result.AuthToken;
        }

        public void LogOut()
        {
            Properties.Settings.Default.AuthToken = null;
        }

        public MeResponse? Me()
        {
            EnsureAuthenticated();
            var response = client.GetAsync("/users/me").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<MeResponse>(json);
        }

        public UsersResponse? GetAllUsers()
        {
            EnsureAuthenticated();
            var response = client.GetAsync("/users/all").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? GetUser(int id)
        {
            EnsureAuthenticated();
            var response = client.GetAsync($"/users/find{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? DeleteUser(int id)
        {
            EnsureAuthenticated();
            var response = client.DeleteAsync($"/users/delete{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? UpdateUser(UpdateUserResponse update)
        {
            EnsureAuthenticated();
            var body = JsonContent.Create<UpdateUserResponse>(update);
            var response = client.PatchAsync("/users/update", body).Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? SearchUser(string x)
        {
            EnsureAuthenticated();
            var response = client.GetAsync($"/users/search{x}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        private void EnsureAuthenticated()
        {
            if (!IsAuthenticated())
            {
                Properties.Settings.Default.AuthToken = null;
                throw new Exception("You are not authenticated!\nPlease log in");
            }
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AuthToken);
        }

        private bool IsAuthenticated()
        {
            string token = Properties.Settings.Default.AuthToken;
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var jt = new JwtSecurityToken(token);
                    return jt.ValidTo > DateTime.Now;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

    }
}
