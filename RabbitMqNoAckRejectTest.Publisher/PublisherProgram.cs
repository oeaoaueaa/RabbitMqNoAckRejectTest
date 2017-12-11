using RabbitMqNoAckRejectTest.Common;
using System;
using System.Text;
using RabbitMQ.Client.Framing;

namespace RabbitMqNoAckRejectTest.Publisher
{
    class PublisherProgram
    {
        static void Main(string[] args)
        {
            string ReadInput()
            {
                Console.Write("message to publish (q:exit)? ");
                return Console.ReadLine();
            };

            var model = Shared.CreateModel();

            var messageBody = ReadInput();
            while (!string.Equals("q", messageBody))
            {
                var messageBodyBytes = Encoding.UTF8.GetBytes(messageBody);
                model.BasicPublish(Shared.Exhange, Shared.RoutingKey, false, new BasicProperties(), messageBodyBytes);
                messageBody = ReadInput();
            }
            Console.WriteLine("End");
        }
    }
}
