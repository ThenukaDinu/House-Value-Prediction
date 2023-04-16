using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using SettingsModels;

namespace Micro_House_Manage_API.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IOptions<RabbitMQSettings> _rabbitMQSettings;
        public MessageProducer(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
        }

        public void SendingMessage<T>(T message, string queue, string routingKey)
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

            channel.QueueDeclare(queue, durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: "", routingKey: routingKey, body: body);
        }
    }
}
