using Micro_Email_Service.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro_Email_Service.Services
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Consume()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "E3C20CE98",
                VirtualHost = "/"
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("emails", durable: true, exclusive: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                // getting byte array[]
                var body = eventArgs.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"A email has been recieved - {message}");
            };

            channel.BasicConsume("emails", autoAck: true, consumer);

            Console.WriteLine("Consumer started. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
