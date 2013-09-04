using Moq;
using NUnit.Framework;
using System.Linq;

namespace DrumScore.Tests
{
    [TestFixture]
    public class ScoreQueueTest
    {
        private ScoreQueue queue;
        private Mock<IScoreReader> reader;

        [SetUp]
        public void Setup()
        {
            reader = new Mock<IScoreReader>();
            queue = new ScoreQueue(reader.Object);
        }

        [Test]
        public void UpdateAddsNewScoresToQueue()
        {
            reader.Setup(s => s.GetLatest()).Returns(new[] { new ScoreInfo { Id = 123 }, new ScoreInfo { Id = 234 } });

            queue.Update();

            Assert.That(queue.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void DuplicateScoresAreNotAddedToQueue()
        {
            var duplicateItem = new ScoreInfo { Id = 123 };
            reader.Setup(s => s.GetLatest()).Returns(new[] { duplicateItem });
            queue.Items.Add(duplicateItem);

            queue.Update();

            Assert.That(queue.Items.Count(s => s.Id == duplicateItem.Id), Is.EqualTo(1));
        }
    }
}