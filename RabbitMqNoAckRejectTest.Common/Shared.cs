using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqNoAckRejectTest.Common
{
    public static class Shared
    {
        public const string Exhange = "RabbitMqNoAckRejectTest";
        public const string RoutingKey = "anything";
        public const string ConsumersQueueName = "ConsumersQueue";

        public static IModel CreateModel()
        {
            IConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                Port = 5672,
                VirtualHost = "/",
            };

            var connection = factory.CreateConnection();
            var model = connection.CreateModel();

            return model;
        }

        public static void Consume(string one)
        {
            var ended = new ManualResetEventSlim();

            var model = CreateModel();

            model.QueueDeclare(ConsumersQueueName, false, false, true);
            model.QueueBind(ConsumersQueueName, Exhange, RoutingKey);

            var consumer = new EventingBasicConsumer(model);

            string consumerTag = null;
            consumer.Received += (ch, ea) =>
            {
                var messageBody = Encoding.UTF8.GetString(ea.Body);
                
                Console.WriteLine($"Received: {messageBody}");

                Console.Write("a:ack/n:noack/nr:noack+requeue/r:reject/rr:rejectrequeue/q:quit/<other>:ignore? ");
                var input = Console.ReadLine();
                if (string.Equals("a", input, StringComparison.OrdinalIgnoreCase))
                {
                    model.BasicAck(ea.DeliveryTag, false);
                }
                else if (string.Equals("n", input, StringComparison.OrdinalIgnoreCase))
                {
                    model.BasicNack(ea.DeliveryTag, false, false);
                }
                else if (string.Equals("nr", input, StringComparison.OrdinalIgnoreCase))
                {
                    model.BasicNack(ea.DeliveryTag, false, true);
                }
                else if (string.Equals("r", input, StringComparison.OrdinalIgnoreCase))
                {
                    model.BasicReject(ea.DeliveryTag, false);
                }
                else if (string.Equals("rr", input, StringComparison.OrdinalIgnoreCase))
                {
                    model.BasicReject(ea.DeliveryTag, true);
                }
                else if (string.Equals("q", input, StringComparison.OrdinalIgnoreCase))
                {
                    if (consumerTag != null)
                        model.BasicCancel(consumerTag);
                    model.Dispose();
                    ended.Set();
                }
            };            

            consumerTag = model.BasicConsume(ConsumersQueueName, false, "", false, false, null, consumer);
            Console.WriteLine($"Consuming ConsumerTag={consumerTag}");

            ended.Wait();
        }
    }
}
