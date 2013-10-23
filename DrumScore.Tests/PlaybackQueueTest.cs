using System.Collections.Generic;
using DrumScore.ScoreSourcing;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests
{
    [TestFixture]
    public class PlaybackQueueTest
    {
        private Mock<ScoreQueue> scoreQueue;
        private Mock<Playback> playback;
        private ScoreInfo scoreToPlay;
        private PlaybackQueue queue;

        [SetUp]
        public void Setup()
        {
            scoreQueue = new Mock<ScoreQueue>(null, null, null);
            playback = new Mock<Playback>(null);
            scoreToPlay = new ScoreInfo();
            queue = new PlaybackQueue(scoreQueue.Object, playback.Object);
        }

        [Test]
        public void PlayPlaysNextScoreInPlaylist()
        {
            scoreQueue.Setup(s => s.GetNextScoreToPlay()).Returns(scoreToPlay);

            queue.Play();

            scoreQueue.Verify(s => s.GetNextScoreToPlay());
            playback.Verify(p => p.Play(scoreToPlay.Score));
        }

        [Test]
        public void OnPlaybackCompleteNextScoreIsPlayed()
        {
            scoreQueue.Setup(s => s.GetNextScoreToPlay()).Returns(scoreToPlay);

            queue.Play();
            playback.Raise(p => p.Complete += null);

            scoreQueue.Verify(s => s.GetNextScoreToPlay(), Times.Exactly(2));
            playback.Verify(p => p.Play(scoreToPlay.Score), Times.Exactly(2));
        }

        [Test]
        public void StopsAndRaisesNotificationWhenAllScoresHaveBeenPlayed()
        {
            scoreQueue.Setup(s => s.GetNextScoreToPlay())
                      .Returns(new Queue<ScoreInfo>(new[] { scoreToPlay, null }).Dequeue);

            playback.Setup(p => p.Play(It.IsAny<IScore>())).Raises(p => p.Complete += null);

            var eventRaised = false;
            queue.Complete += () => eventRaised = true;

            queue.Play();

            Assert.That(eventRaised);
        }
    }
}
