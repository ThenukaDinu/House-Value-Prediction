using Models.Requests;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Micro_House_Manage_API.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;
        private readonly HttpClient _httpClient;
        public HttpClientService(ILogger<HttpClientService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostAsync(string url,  string json)
        {
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured {ex}", ex);
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured {ex}", ex);
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url, string accessToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured {ex}", ex);
                throw;
            }
        }

        public async Task<string> ReadAsStringAsync(HttpResponseMessage response)
        {
            try
            {
                var message = await response.Content.ReadAsStringAsync();
                return message;
            }
            catch (Exception ex)
            {
                 _logger.LogError("An error occured {ex}", ex);
                throw;
            }
        }
    }
}
