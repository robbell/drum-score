using NUnit.Framework;
using System.Linq;

namespace DrumScore.Tests
{
    [TestFixture]
    public class ScoreTest
    {
        [Test]
        public void AddSampleAddsSampleAtCurrentPosition()
        {
            var sample = new Sample("*");
            var score = new Score();

            score.AddSample(sample);

            Assert.That(score.Samples[0].Single(), Is.EqualTo(sample));
        }

        [Test]
        public void MultipleDifferentSamplesCanBeAddedAtPosition()
        {
            var firstSample = new Sample("*");
            var secondSample = new Sample("=");
            var score = new Score();

            score.AddSample(firstSample);
            score.AddSample(secondSample);

            Assert.IsTrue(score.Samples[0].Contains(firstSample));
            Assert.IsTrue(score.Samples.First().Value.Contains(secondSample));
        }

        [Test]
        public void IdenticalSampleIsNotDuplicated()
        {
            const string key = "*";
            var sample = new Sample(key);
            var identicalSample = new Sample(key);
            var score = new Score();

            score.AddSample(sample);
            score.AddSample(identicalSample);

            Assert.That(score.Samples[0].Count(s => s.Type == key), Is.EqualTo(1));
        }

        [Test]
        public void SamplesAreAddedEightFramesApartAfterProgression()
        {
            var firstSample = new Sample("*");
            var secondSample = new Sample("=");
            var score = new Score();

            score.AddSample(firstSample);
            score.Progress();
            score.AddSample(secondSample);

            Assert.That(score.Samples[0].Contains(firstSample));
            Assert.That(score.Samples[8].Contains(secondSample));
        }

        [TestCase(3)]
        [TestCase(-2)]
        public void SetPositionAllowsSamplesToBeAddedBeforeAndAfterTheBeat(int adjustment)
        {
            var sample = new Sample("*");
            var score = new Score();

            score.Progress(); // position will now be eight
            score.SetPosition(adjustment);
            score.AddSample(sample);

            const int initialPosition = 8;
            Assert.That(score.Samples[initialPosition + adjustment].Contains(sample));
        }

        [TestCase(3, 16, 24)]
        [TestCase(6, 16, 24)]
        [TestCase(-2, 8, 16)]
        [TestCase(-3, 8, 16)]
        public void ProgressAfterSetPositionMovesToNextBeat(int adjustment, int expectedNextPosition, int expectedLastPosition)
        {
            var dummySample = new Sample("=");
            var expectedSample = new Sample("=");
            var score = new Score();

            score.Progress(); // position will now be eight
            score.SetPosition(adjustment);
            score.AddSample(dummySample);
            score.Progress();
            score.AddSample(expectedSample);
            score.Progress();
            score.AddSample(expectedSample);

            Assert.That(score.Samples[expectedNextPosition].Contains(expectedSample));
            Assert.That(score.Samples[expectedLastPosition].Contains(expectedSample));
        }
    }
}
