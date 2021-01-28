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
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var counter = 0;

            channel.QueueDeclare("hello", false, false, false, null);

            while (true)
            {
                counter++;

                var item = new LotItem
                {
                    Date = DateTime.UtcNow,
                    Contract = counter,
                    Lot = counter,
                    Price = counter * 100m
                };
                var message = JsonConvert.SerializeObject(item);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", "hello", null, body);

                Console.WriteLine(message);
            }

        }
    }
}
