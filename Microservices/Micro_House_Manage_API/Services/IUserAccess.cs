using Models.Others;

namespace Micro_House_Manage_API.Services
{
    public interface IUserAccess
    {
        public Task<UserInfo> GetUserProfile(string accessToken);
        public Task<string> GetTokenAsync(string scheme, string tokenName, HttpContext httpContext);
    }
}
