using System;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore.DummySender
{
    public class Program
    {
        private static readonly IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 12015);

        public static void Main()
        {
            Console.WriteLine("Dummy Sender started.");

            while (true)
            {
                Console.WriteLine("Type message body and press enter:");
                SendMessage(Console.ReadLine());

                Console.WriteLine("Message sent.");
                Console.WriteLine();
            }
        }

        private static void SendMessage(string messageText)
        {
            var message = new OscMessage(endPoint, "/");

            message.Append(messageText);

            message.Send(endPoint);
        }
    }
}
