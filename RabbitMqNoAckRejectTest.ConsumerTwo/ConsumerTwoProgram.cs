using RabbitMqNoAckRejectTest.Common;
using System;

namespace RabbitMqNoAckRejectTest.ConsumerTwo
{
    class ConsumerTwoProgram
    {
        static void Main(string[] args)
        {
            Shared.Consume("Two");
            Console.WriteLine("End");
        }
    }
}
