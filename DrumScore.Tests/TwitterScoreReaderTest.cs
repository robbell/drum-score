using NUnit.Framework;

namespace DrumScore.Tests
{
    [TestFixture]
    public class TwitterScoreReaderTest
    {
        [Test]
        public void GetsLatestScoreInfoFromTwitter()
        {
            var reader = new TwitterScoreReader();
            Assert.That(reader.GetLatest(), Is.Not.Empty);
        }
    }
}
