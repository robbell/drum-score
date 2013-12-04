using DrumScore.Interpretation;
using DrumScore.ScoreSourcing;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DrumScore.Tests.ScoreSourcing
{
    [TestFixture]
    public class ScoreQueueTest
    {
        private ScoreQueue queue;
        private Mock<IScoreFeed> feed;
        private Mock<INotifications> notifications;
        private Mock<Interpreter> interpreter;

        [SetUp]
        public void Setup()
        {
            feed = new Mock<IScoreFeed>();
            notifications = new Mock<INotifications>();
            interpreter = new Mock<Interpreter>(null);
            queue = new ScoreQueue(feed.Object, interpreter.Object, notifications.Object);
        }

        [Test]
        public void UpdateAddsNewScoresToQueue()
        {
            queue.ScoreReceived(new ScoreInfo { Id = 123 });
            queue.ScoreReceived(new ScoreInfo { Id = 234 });

            Assert.That(queue.Tweets.Count, Is.EqualTo(2));
        }

        [Test, Ignore("No longer relevant as scores are pushed on publish from Twitter")]
        public void DuplicateScoresAreNotAddedToQueue()
        {
            var duplicateItem = new ScoreInfo { Id = 123 };
            queue.Tweets.Add(duplicateItem);

            queue.ScoreReceived(duplicateItem);

            Assert.That(queue.Tweets.Count(s => s.Id == duplicateItem.Id), Is.EqualTo(1));
        }

        [Test]
        public void SubmitterIsNotifiedOfInvalidScore()
        {
            var invalidScore = new ScoreInfo { Id = 123, TextScore = "BadScore", Username = "MrMan" };

            var expectedException = new UnrecognisedTokenException("Invalid Token");
            interpreter.Setup(i => i.Interpret(invalidScore.TextScore)).Throws(expectedException);

            queue.ScoreReceived(invalidScore);

            notifications.Verify(n => n.SendError(invalidScore, expectedException), Times.Once());
            Assert.That(queue.Tweets, Is.Empty);
        }

        [Test]
        public void ScoreInfoIsMovedFromTweetListToBottomOfPlaylist()
        {
            var scoreToMove = new ScoreInfo { Id = 123 };

            queue.Tweets.Add(new ScoreInfo());
            queue.Tweets.Add(scoreToMove);
            queue.Tweets.Add(new ScoreInfo());

            queue.Playlist.Add(new ScoreInfo());
            queue.Playlist.Add(new ScoreInfo());

            queue.MoveToPlaylist(scoreToMove);

            Assert.That(queue.Playlist.Last(), Is.EqualTo(scoreToMove));
            Assert.That(queue.Tweets.Contains(scoreToMove), Is.False);
        }

        [Test]
        public void ScoreInfoCanBeMovedUpAndDownPlaylist()
        {
            var scoreToMove = new ScoreInfo { Id = 123 };

            queue.Playlist.Add(new ScoreInfo());
            queue.Playlist.Add(scoreToMove);
            queue.Playlist.Add(new ScoreInfo());

            queue.MoveItemUp(scoreToMove);

            Assert.That(queue.Playlist[0], Is.EqualTo(scoreToMove));

            queue.MoveItemDown(scoreToMove);

            Assert.That(queue.Playlist[1], Is.EqualTo(scoreToMove));
        }

        [Test]
        public void ScoreAtTopOfPlaylistIsntMoved()
        {
            var scoreToMove = new ScoreInfo { Id = 123 };

            queue.Playlist.Add(scoreToMove);
            queue.Playlist.Add(new ScoreInfo());

            queue.MoveItemUp(scoreToMove);

            Assert.That(queue.Playlist.First(), Is.EqualTo(scoreToMove));
        }

        [Test]
        public void ScoreAtBottomOfPlaylistIsntMoved()
        {
            var scoreToMove = new ScoreInfo { Id = 123 };

            queue.Playlist.Add(new ScoreInfo());
            queue.Playlist.Add(scoreToMove);

            queue.MoveItemDown(scoreToMove);

            Assert.That(queue.Playlist.Last(), Is.EqualTo(scoreToMove));
        }

        [Test]
        public void MovesTopTweetScoreToPlaylist()
        {
            var scoreToMove = new ScoreInfo { Id = 123 };

            queue.Tweets.Add(scoreToMove);
            queue.Tweets.Add(new ScoreInfo());
            queue.Tweets.Add(new ScoreInfo());

            queue.MoveTopTweetToPlaylist();

            Assert.That(queue.Playlist.Contains(scoreToMove));
            Assert.That(queue.Tweets.Contains(scoreToMove), Is.False);
        }

        [Test]
        public void DoesntAttemptToMoveIfTweetQueueIsEmpty()
        {
            queue.MoveTopTweetToPlaylist();

            Assert.That(queue.Playlist, Is.Empty);
        }
    }
}