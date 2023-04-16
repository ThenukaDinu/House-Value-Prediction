using Micro_Email_Service.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SettingsModels;
using Models;
using System.Text.Json;

namespace Micro_Email_Service.Services
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IConfiguration _config;
        private readonly IEmailSendService _emailSendService;

        public MessageConsumer(ILogger<MessageConsumer> logger, IConfiguration config, IEmailSendService emailSendService)
        {
            _logger = logger;
            _config = config;
            _emailSendService = emailSendService;
        }

        public void Consume()
        {
            _logger.LogInformation("Consume starting...");
            var rabbitMqSettings = _config.GetSection("RabbitMQ").Get<RabbitMQSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.HostName,
                UserName = rabbitMqSettings.UserName,
                Password = rabbitMqSettings.Password,
                VirtualHost = rabbitMqSettings.VirtualHost
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("emails", durable: true, exclusive: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                _logger.LogInformation("Recived new message");

                // getting byte array[]
                var body = eventArgs.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);
                string info = $"A email has been recieved - {message}";
                _logger.LogInformation(info);
                Console.WriteLine(info);

                EmailMessage email = JsonSerializer.Deserialize<EmailMessage>(message);
                MailSettings mailSettings = _config.GetSection("MailSettings").Get<MailSettings>();
                _emailSendService.Send(mailSettings, email.Body, email.Subject, email.To);
            };

            channel.BasicConsume("emails", autoAck: true, consumer);

            Console.WriteLine("Consumer started. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
