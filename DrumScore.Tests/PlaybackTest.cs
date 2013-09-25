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
            var samplesAtPosition1 = new[] {new Sample("=")};
            var samplesAtPosition2 = new[] {new Sample("=")};
            var samplesAtPosition3 = new[] {new Sample("=")};

            var score = new Mock<IScore>();
            score.Setup(s => s.Samples.Count).Returns(3);
            score.Setup(s => s.Samples[0]).Returns(samplesAtPosition1);
            score.Setup(s => s.Samples[1]).Returns(samplesAtPosition2);
            score.Setup(s => s.Samples[2]).Returns(samplesAtPosition3);

            var output = new Mock<IPlaybackOutput>();

            var player = new Playback(output.Object);
            player.Play(score.Object);

            output.Verify(o => o.Play(samplesAtPosition1));
            output.Verify(o => o.Play(samplesAtPosition2));
            output.Verify(o => o.Play(samplesAtPosition3));
        }
    }
}
