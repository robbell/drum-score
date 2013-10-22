using DrumScore.ScoreSourcing;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests
{
    [TestFixture]
    public class PlaybackQueueTest
    {
        [Test]
        public void PlayPlaysNextScoreInPlaylist()
        {
            var scoreQueue = new Mock<ScoreQueue>(null, null, null);
            var playback = new Mock<Playback>(null);
            var scoreToPlay = new ScoreInfo();

            var queue = new PlaybackQueue(scoreQueue.Object, playback.Object);

            scoreQueue.Setup(s => s.GetNextScoreToPlay()).Returns(scoreToPlay);

            queue.Play();

            scoreQueue.Verify(s => s.GetNextScoreToPlay());
            playback.Verify(p => p.Play(scoreToPlay.Score));
        }
    }
}
