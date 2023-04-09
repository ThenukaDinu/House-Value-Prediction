namespace Micro_House_Manage_API.Services
{
    public interface IMessageProducer
    {
        public void SendingMessage<T>(T message);
    }
}
