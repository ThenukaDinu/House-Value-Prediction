namespace Micro_House_Manage_API.Services
{
    public interface IHttpClientService
    {
        public Task<HttpResponseMessage> PostAsync(string url, string json);
        public Task<string> ReadAsStringAsync(HttpResponseMessage response);
        public Task<HttpResponseMessage> GetAsync(string url);
        public Task<HttpResponseMessage> GetAsync(string url, string accessToken);
    }
}
