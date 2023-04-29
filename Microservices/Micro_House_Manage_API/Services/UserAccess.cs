using Models.Others;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace Micro_House_Manage_API.Services
{
    public class UserAccess : IUserAccess
    {
        private readonly HttpClient _httpClient;

        public UserAccess(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<UserInfo> GetUserProfile(string accessToken)
        {
            // Set the authorization header to include the access token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Make a request to the user info endpoint
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:44342/connect/userinfo");

            // Deserialize the JSON response into a C# object
            string jsonContent = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserInfo>(jsonContent);

            return userInfo;
        }
    }
}
