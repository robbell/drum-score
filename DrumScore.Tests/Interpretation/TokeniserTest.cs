using DrumScore.Interpretation;
using DrumScore.Interpretation.Expressions;
using NUnit.Framework;
using System.Linq;

namespace DrumScore.Tests.Interpretation
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

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("4")]
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
            var expression = tokeniser.ReadTokens("1 . 2");

            Assert.That(expression[0], Is.TypeOf<SampleExpression>());
            Assert.That(expression[1], Is.TypeOf<SkipBeatExpression>());
            Assert.That(expression[2], Is.TypeOf<SampleExpression>());
        }

        [Test]
        public void ConsecutiveSampleTokensAreGroupedAsSimultaneousSampleExpression()
        {
            var expression = tokeniser.ReadTokens("123").First();

            Assert.That(expression, Is.TypeOf<SampleExpression>());
            Assert.That(((SampleExpression)expression).Sample, Is.EqualTo("123"));
        }

        [Test]
        public void EarlyOffsetTokenCreatesEarlyOffsetSampleWithDefaultOffset()
        {
            var expression = tokeniser.ReadTokens(@"\4").First();

            Assert.That(expression, Is.TypeOf<OffsetSampleExpression>());
        }

        [Test]
        public void LateOffsetTokenCreatesLateOffsetSampleWithDefaultOffset()
        {
            var expression = tokeniser.ReadTokens(@"/2").First();

            Assert.That(expression, Is.TypeOf<OffsetSampleExpression>());
        }

        [TestCase(1)]
        [TestCase(7)]
        public void OffsetTokenCreatesOffsetSampleWithCorrectOffset(int offset)
        {
            var score = string.Format(@"{0}\3", offset);

            var expression = tokeniser.ReadTokens(score).First();

            Assert.That(expression, Is.TypeOf<OffsetSampleExpression>());
        }

        [Test]
        [ExpectedException(typeof(UnrecognisedTokenException))]
        public void UnrecognisedTokenThrowsException()
        {
            const string badScore = "12.";

            tokeniser.ReadTokens(badScore);
        }
    }
}
