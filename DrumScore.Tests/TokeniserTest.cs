using DrumScore.Expressions;
using NUnit.Framework;
using System.Linq;

namespace DrumScore.Tests
{
    [TestFixture]
    public class TokeniserTest
    {
        private Tokeniser tokeniser;

        [SetUp]
        public void Setup()
        {
            tokeniser = new Tokeniser();
        }

        [TestCase("^")]
        [TestCase("=")]
        [TestCase("o")]
        [TestCase("*")]
        public void SampleTokenReturnsSampleExpression(string score)
        {
            var expression = tokeniser.ReadTokens(score).First();

            Assert.That(expression, Is.TypeOf<SampleExpression>());
            Assert.That(((SampleExpression)expression).Sample, Is.EqualTo(score));
        }

        [Test]
        public void SkipBeatTokenReturnsSkipBeatExpression()
        {
            var expression = tokeniser.ReadTokens(".");

            Assert.That(expression.First(), Is.TypeOf<SkipBeatExpression>());
        }

        [Test]
        public void MixedScoreCreatesMixedExpressions()
        {
            var expression = tokeniser.ReadTokens("* . o");

            Assert.That(expression[0], Is.TypeOf<SampleExpression>());
            Assert.That(expression[1], Is.TypeOf<SkipBeatExpression>());
            Assert.That(expression[2], Is.TypeOf<SampleExpression>());
        }

        [Test]
        public void ConsecutiveSampleTokensAreGroupedAsSimultaneousSampleExpression()
        {
            var expression = tokeniser.ReadTokens("*=^").First();

            Assert.That(expression, Is.TypeOf<SampleExpression>());
            Assert.That(((SampleExpression)expression).Sample, Is.EqualTo("*=^"));
        }

        [Test]
        public void EarlyOffsetTokenCreatesEarlyOffsetSampleWithDefaultOffset()
        {
            var expression = tokeniser.ReadTokens(@"\*").First();

            Assert.That(expression, Is.TypeOf<OffsetSampleExpression>());
            Assert.That(((OffsetSampleExpression)expression).Offset, Is.EqualTo(1));
            Assert.That(((OffsetSampleExpression)expression).Timing, Is.EqualTo(OffsetTiming.Early));
        }

        [Test]
        public void LateOffsetTokenCreatesLateOffsetSampleWithDefaultOffset()
        {
            var expression = tokeniser.ReadTokens(@"/*").First();

            Assert.That(expression, Is.TypeOf<OffsetSampleExpression>());
            Assert.That(((OffsetSampleExpression)expression).Offset, Is.EqualTo(1));
            Assert.That(((OffsetSampleExpression)expression).Timing, Is.EqualTo(OffsetTiming.Late));
        }

        [TestCase(1, Result = 1)]
        [TestCase(8, Result = 8)]
        public int OffsetTokenCreatesOffsetSampleWithCorrectOffset(int offset)
        {
            var score = string.Format(@"{0}\*", offset);

            var expression = tokeniser.ReadTokens(score).First();

            return ((OffsetSampleExpression)expression).Offset;
        }
    }
}
