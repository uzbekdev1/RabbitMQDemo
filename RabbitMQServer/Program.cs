using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon;

namespace RabbitMQServer
{
    internal class Program
    {
        private static readonly AutoResetEvent _waiter = new AutoResetEvent(false);

        private static void Main()
        {

            Console.Title = "Server";

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("hello", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ConsumerOnReceived;

            channel.BasicConsume("hello", true, consumer);

            _waiter.WaitOne();
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var item = JsonConvert.DeserializeObject<LotItem>(message);
            var dateNow = DateTime.UtcNow;

            Console.WriteLine(dateNow.Subtract(item.Date).ToString("g"));
        }

    }
}
