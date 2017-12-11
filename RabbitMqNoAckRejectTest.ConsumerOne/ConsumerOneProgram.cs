using RabbitMqNoAckRejectTest.Common;
using System;

namespace RabbitMqNoAckRejectTest.ConsumerOne
{
    class ConsumerOneProgram
    {
        static void Main(string[] args)
        {
            Shared.Consume("One");
            Console.WriteLine("End");
        }
    }
}
