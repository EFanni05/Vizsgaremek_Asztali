using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        class UserRoleUpdate
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("role")]
            public string UserRole { get; set; }
            public UserRoleUpdate(int id, string userRole)
            {
                Id = id;
                UserRole = userRole;
            }

        }

        class LoginResponse
        {
            [JsonProperty("token")]
            public string? AuthToken { get; set; }

            [JsonProperty("user_id")]
            public int UserId { get; set; }
        }

        private static HttpClient _client = new HttpClient();
        
        public APIClient(string baseAddress) 
        {
            _client.BaseAddress = new Uri(baseAddress);
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }


        public void Login(string email, string password)
        {
            LogOut();
            var body = JsonContent.Create<LoginRequest>(new LoginRequest(email, password));
            var response = _client.PostAsync("/auth/login", body).Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<LoginResponse>(json);
            if (result?.AuthToken == null)
            {
                throw new ApplicationException("API error (invalid response or token is received)");
            }
            Properties.Settings.Default.AuthToken = result.AuthToken;
            Properties.Settings.Default.CurrentUserId = result.UserId;
        }

        public void LogOut()
        {
            Properties.Settings.Default.AuthToken = null;
        }

        public MeResponse? Me()
        {
            EnsureAuthenticated();
            var response = _client.GetAsync("/users/me").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<MeResponse>(json);
        }

        public IList<UsersResponse>? GetAllUsers()
        {
            EnsureAuthenticated();
            var response = _client.GetAsync("/users/all").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IList<UsersResponse>>(json);
        }

        public UsersResponse? GetUser(int id)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/users/find{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? DeleteUser(int id)
        {
            EnsureAuthenticated();
            var response = _client.DeleteAsync($"/users/delete{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? UpdateMe(UpdateUserResponse update)
        {
            EnsureAuthenticated();
            var body = JsonContent.Create<UpdateUserResponse>(update);
            var response = _client.PatchAsync($"/users/update{update.Id}", body).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                // TODO: properly formatted error details
                throw new HttpRequestException($"Failed to update user: {data}");
            }
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public UsersResponse? UpdateAdminUser(UpdateUserResponse update)
        {
            EnsureAuthenticated();
            var body = JsonContent.Create<UpdateUserResponse>(update);
            var response = _client.PatchAsync($"/users/update-admin/{Properties.Settings.Default.CurrentUserId}", body).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                throw new HttpRequestException($"Failed to update user: {data}");
            }
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<UsersResponse>(json);
        }

        public IList<UsersResponse>? SearchUser(string searchString)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/users/search{searchString}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IList<UsersResponse>>(json);
        }

        public IList<RecipesResponse>? SearchUser(int id)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/recipes/search-user{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IList<RecipesResponse>>(json);
        }

        public IList<RecipesResponse>? GetAllRecipes()
        {
            EnsureAuthenticated();
            var response = _client.GetAsync("/recipes/all").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IList<RecipesResponse>>(json);
        }

        public RecipesResponse? SearchRecipe(int id)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/recipes/find{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RecipesResponse>(json);
        }

        public RecipesResponse? UpdateRecipe(UpdateRecipes update)
        {
            EnsureAuthenticated();
            var body = JsonContent.Create<UpdateRecipes>(update);
            var response = _client.PatchAsync($"/recipes/update{update.Id}", body).Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RecipesResponse>(json);
        }

        public RecipesResponse? DeleteRecipe(int id)
        {
            EnsureAuthenticated();
            var response = _client.DeleteAsync($"/recipes/delete-admin/{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RecipesResponse>(json);
        }

        public IList<RatingResponse>? GetAllRating()
        {
            EnsureAuthenticated();
            var response = _client.GetAsync("/ratings/getAll").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IList<RatingResponse>>(json);
        }

        public RatingResponse? GetRating(int id)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/ratings/find{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RatingResponse>(json);
        }

        public RatingResponse? UpdateRating(RatingUpdateResponse update)
        {
            EnsureAuthenticated();
            var body = JsonContent.Create<RatingUpdateResponse>(update);
            var response = _client.PatchAsync($"/ratings/updateAdmin{update.Id}", body).Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RatingResponse>(json);
        }

        public RatingResponse? DeleteRating(int id)
        {
            EnsureAuthenticated();
            var response = _client.DeleteAsync($"/ratings/delete-admin/{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RatingResponse>(json);
        }

        public string? GetAllergensForRecipe(int id)
        {
            EnsureAuthenticated();
            var response = _client.GetAsync($"/allergens/find-recipe{id}").Result;
            response.EnsureSuccessStatusCode();
            var json = response.Content.ReadAsStringAsync();
            return JsonConvert.SerializeObject(json);
        }

        private void EnsureAuthenticated()
        {
            if (!IsAuthenticated())
            {
                Properties.Settings.Default.AuthToken = null;
                throw new UnauthenticatedException();
            }
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AuthToken);
            }
        }

        private bool IsAuthenticated()
        {
            string token = Properties.Settings.Default.AuthToken;
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var jt = new JwtSecurityToken(token);
                    return jt.ValidTo > DateTime.UtcNow;
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
