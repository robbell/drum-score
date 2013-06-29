using DrumScore.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests.Expressions
{
    [TestFixture]
    public class SkipBeatExpressionTest
    {
        [Test]
        public void ExpressionProgressesScoreToNextBeat()
        {
            var score = new Mock<IScore>();
            var expression = new SkipBeatExpression();

            expression.Interpret(score.Object);

            score.Verify(s => s.Progress());
        }
    }
}