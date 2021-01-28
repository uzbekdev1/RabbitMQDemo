using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQCommon;

namespace RabbitMQClient
{
    internal class Program
    {
        private static void Main()
        {

            Console.Title = "Client";

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel(); 

            channel.QueueDeclare("hello", false, false, false, null);

            while (true)
            {
                var counter = new Random().Next(1, 1000000);
                var item = new LotItem
                {
                    Date = DateTime.UtcNow,
                    Contract = counter,
                    Lot = counter,
                    Price = counter * 100m,
                    User = $"{Guid.NewGuid()}"
                };
                var message = JsonConvert.SerializeObject(item);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", "hello", null, body);

                Console.WriteLine(message);
            }

        }
    }
}
