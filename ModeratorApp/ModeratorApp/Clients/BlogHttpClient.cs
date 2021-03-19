using ModeratorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace ModeratorApp.Clients
{
    public class BlogHttpClient : IBlogHttpClient
    {
        private HttpClient _httpClient = new HttpClient();
        private JsonMediaTypeFormatter _formatter = new JsonMediaTypeFormatter();
        private string authPath = "/api/auth";
        private string usersPath = "/api/admin/users";
        public bool TokenExpired  { get => expirationDate <= DateTime.Now; }
        private DateTime expirationDate;

        public BlogHttpClient(string blogUrl)
        {
            _httpClient.BaseAddress = new Uri(blogUrl);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var response = await _httpClient.GetAsync(usersPath);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<User>>();
            }
            else throw new ArgumentException("Couldn't retrieve users");
        }

        public async Task Authenticate(User admin)
        {
            var response = await _httpClient.PostAsync<User>(authPath, admin, _formatter);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsAsync<AuthResponse>();
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + content.Auth_token);
                expirationDate = DateTime.Now.AddSeconds(content.Expires_in);
            }
            else throw new ArgumentException("Authorization failed");
        }
    }
}
