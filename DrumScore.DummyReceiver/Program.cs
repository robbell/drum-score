using System;
using System.Configuration;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore.DummyReceiver
{
    public class Program
    {
        public static void Main()
        {
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback,
                                       Convert.ToInt32(ConfigurationManager.AppSettings["ListenOnPort"]));
            server.RegisterMethod("/");
            server.MessageReceived += MessageReceived;
            server.Start();

            Console.WriteLine("Dummy Receiver started.");
            Console.WriteLine("Awaiting messages...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            server.Stop();
        }

        private static void MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            foreach (var sample in e.Message.Data)
            {
                Console.Write(sample);
            }

            Console.WriteLine();
        }
    }
}
