using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore.DummySender
{
    public class Program
    {
        private static readonly IPEndPoint endPoint =
            new IPEndPoint(IPAddress.Parse(ConfigurationManager.AppSettings["SendToIp"]),
                           Convert.ToInt32(ConfigurationManager.AppSettings["SendOnPort"]));

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

            foreach (var item in messageText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.Append(item);
            }

            message.Send(endPoint);
        }
    }
}
