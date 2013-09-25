using System;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore.DummyReceiver
{
    public class Program
    {
        public static void Main()
        {
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback, 12013);
            server.MessageReceived += MessageReceived;
            server.Start();

            Console.WriteLine("DummyReceiver Started");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            server.Stop();
        }

        private static void MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
