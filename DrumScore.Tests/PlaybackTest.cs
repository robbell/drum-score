using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests
{
    [TestFixture]
    public class PlaybackTest
    {
        [Test]
        public void SamplesArePlayedToOutput()
        {
            var samplesAtPosition1 = new[] { new Sample("=") };
            var samplesAtPosition2 = new[] { new Sample("=") };
            var samplesAtPosition3 = new[] { new Sample("=") };

            var score = new Mock<IScore>();
            score.Setup(s => s.Samples)
                 .Returns(new Dictionary<int, ICollection<Sample>>
                     {
                         { 0, samplesAtPosition1 },
                         { 8, samplesAtPosition2 },
                         { 16, samplesAtPosition3 }
                     });

            var output = new Mock<IPlaybackOutput>();

            var player = new Playback(output.Object);

            player.Complete += () =>
                {
                    output.Verify(o => o.Play(samplesAtPosition1));
                    output.Verify(o => o.Play(samplesAtPosition2));
                    output.Verify(o => o.Play(samplesAtPosition3));
                };

            player.Play(new ScoreInfo { Score = score.Object });
        }
    }
}
