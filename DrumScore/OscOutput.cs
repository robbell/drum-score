using System;
using System.Collections.Generic;
using System.Net;
using Bespoke.Common.Osc;

namespace DrumScore
{
    public class OscOutput : IPlaybackOutput
    {
        private readonly IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 12013);

        public void PlayScoreStart(string creator)
        {
            SendMessage(m => m.Append(string.Format("BOM {0}", creator)));
        }

        public void PlayScoreEnd()
        {
            SendMessage(m => m.Append("EOM"));
        }

        public void Play(ICollection<Sample> samples)
        {
            SendMessage(m =>
                {
                    foreach (var sample in samples)
                    {
                        m.Append(sample.Type);
                    }
                });
        }

        private void SendMessage(Action<OscMessage> appendAction)
        {
            var message = new OscMessage(endPoint, "/");

            appendAction(message);

            message.Send(endPoint);
        }
    }
}