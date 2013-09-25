using DrumScore.Interpretation.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests.Interpretation.Expressions
{
    [TestFixture]
    public class SampleExpressionTest
    {
        [Test]
        public void ExpressionAddsSingleSampleAndProgressesToNextBeat()
        {
            const string sampleKey = "*";
            var score = new Mock<IScore>();
            var expression = new SampleExpression(sampleKey);

            expression.Interpret(score.Object);

            score.Verify(sc => sc.AddSample(It.Is<Sample>(sm => sm.Type == sampleKey)), Times.Once());
            score.Verify(sc => sc.Progress(), Times.Once());
        }

        [Test]
        public void ExpressionWithMultipleKeysAddMultipleSamplesToScore()
        {
            const string firstKey = "^";
            const string secondKey = "=";

            var score = new Mock<IScore>();
            var expression = new SampleExpression(firstKey + secondKey);

            expression.Interpret(score.Object);

            score.Verify(sc => sc.AddSample(It.Is<Sample>(sm => sm.Type == firstKey)), Times.Once());
            score.Verify(sc => sc.AddSample(It.Is<Sample>(sm => sm.Type == secondKey)), Times.Once());
            score.Verify(sc => sc.Progress(), Times.Once());
        }
    }
}