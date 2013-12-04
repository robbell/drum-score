using System;
using System.Configuration;
using System.Net;
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
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback, Convert.ToInt32(ConfigurationManager.AppSettings["ListenOnPort"]));
            server.RegisterMethod("/");
            server.MessageReceived += MessageReceived;
            server.Start();
        }

        private void MessageReceived(object sender, OscMessageReceivedEventArgs e) // ToDo: This could really do with some unit tests
        {
            var channelName = e.Message.Data.First().ToString();
            var channel = GetPlaybackChannel(channelName);
            var isLoopMessage = e.Message.Data.Last().ToString().ToUpper() == "LOOP";

            if (isLoopMessage)
            {
                if (channel.IsPlaying) channel.ToggleLooping();
            }
            else
            {
                var score = scoreQueue.GetNextScoreToPlay();
                if (score != null) channel.Play(score, Convert.ToInt32(e.Message.Data.Last()));
            }
        }

        private Playback GetPlaybackChannel(string channelName)
        {
            return channelName.ToUpper() == "CH1" ? channel1 : channel2;
        }
    }
}