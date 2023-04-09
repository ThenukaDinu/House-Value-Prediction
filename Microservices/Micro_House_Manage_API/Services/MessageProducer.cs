using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Micro_House_Manage_API.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IOptions<RabbitMQSettings> _rabbitMQSettings;
        public MessageProducer(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public void SendingMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.Value.HostName,
                UserName = _rabbitMQSettings.Value.UserName,
                Password = _rabbitMQSettings.Value.Password,
                VirtualHost = _rabbitMQSettings.Value.VirtualHost
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("send-emails", durable: true, exclusive: true);


        }
    }
}
