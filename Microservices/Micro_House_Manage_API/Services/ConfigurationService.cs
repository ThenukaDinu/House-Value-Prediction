namespace Micro_House_Manage_API.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetSingleValue<T>(string pathToSetting)
        {
            return _configuration.GetValue<T>(pathToSetting);
        }
    }
}
