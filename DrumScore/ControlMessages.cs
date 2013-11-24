using System.Net;
using System.Net.Mime;
using Bespoke.Common.Osc;
using DrumScore.ScoreSourcing;
using System.Linq;

namespace DrumScore
{
    public class ControlMessages
    {
        private readonly ScoreQueue scoreQueue;
        private readonly Playback channel1;
        private readonly Playback channel2;

        public ControlMessages(ScoreQueue scoreQueue, Playback channel1, Playback channel2)
        {
            this.scoreQueue = scoreQueue;
            this.channel1 = channel1;
            this.channel2 = channel2;
        }

        public void Initialise()
        {
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback, 12015);
            server.RegisterMethod("/");
            server.MessageReceived += MessageReceived;
            server.Start();
        }

        private void MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            var messageParts = e.Message.Data.First().ToString().Split(' ');

            var channel = GetPlaybackChannel(messageParts.First());

            if (channel.IsPlaying) return;

            var score = scoreQueue.GetNextScoreToPlay();

            if (score != null) channel.Play(score);
        }

        private Playback GetPlaybackChannel(string channelName)
        {
            return channelName == "CH1" ? channel1 : channel2;
        }
    }
}