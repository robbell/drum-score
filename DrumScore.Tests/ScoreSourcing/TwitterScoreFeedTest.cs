using DrumScore.ScoreSourcing;
using NUnit.Framework;

namespace DrumScore.Tests.ScoreSourcing
{
    [TestFixture]
    public class TwitterScoreFeedTest
    {
        [Test]
        public void GetsLatestScoreInfoFromTwitter()
        {
            var feed = new TwitterScoreFeed();
            Assert.That(feed.GetLatest(), Is.Not.Empty);
        }
    }
}
