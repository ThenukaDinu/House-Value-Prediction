namespace Micro_House_Manage_API.Services
{
    public interface IConfigurationService
    {
        public T GetSingleValue<T>(string pathToSetting);
    }
}
