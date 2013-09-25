using System.Collections.Generic;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore
{
    public class OscOutput : IPlaybackOutput
    {
        private readonly IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 12013);

        public void Play(ICollection<Sample> samples)
        {
            var message = new OscMessage(endPoint, "/");

            foreach (var sample in samples)
            {
                message.Append(sample.Type);
            }

            message.Send(endPoint);
        }
    }
}