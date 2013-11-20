using DrumScore.Interpretation.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests.Interpretation.Expressions
{
    [TestFixture]
    public class OffsetSampleExpressionTest
    {
        [Test]
        public void EarlyExpressionMovesScorePositionBackwards()
        {
            var score = new Mock<IScore>();
            var expression = new OffsetSampleExpression("\\2");

            expression.Interpret(score.Object);

            score.Verify(s => s.SetPosition(-1));
            score.Verify(s => s.AddSample(It.Is<Sample>(b => b.Type == "2")), Times.Once());
            score.Verify(s => s.Progress());
        }

        [Test]
        public void LateExpressionMovesScorePositionForwards()
        {
            var score = new Mock<IScore>();
            var expression = new OffsetSampleExpression("/23");

            expression.Interpret(score.Object);

            score.Verify(s => s.SetPosition(1));
            score.Verify(s => s.AddSample(It.Is<Sample>(b => b.Type == "2")), Times.Once());
            score.Verify(s => s.AddSample(It.Is<Sample>(b => b.Type == "3")), Times.Once());
            score.Verify(s => s.Progress());
        }

        [TestCase("2/", 2)]
        [TestCase("3\\", -3)]
        public void ExpressionMovesScoreCorrectNumberOfPlaces(string token, int expectedOffset)
        {
            var score = new Mock<IScore>();
            var expression = new OffsetSampleExpression(token + "4");

            expression.Interpret(score.Object);

            score.Verify(s => s.SetPosition(expectedOffset));
            score.Verify(s => s.AddSample(It.Is<Sample>(b => b.Type == "4")), Times.Once());
            score.Verify(s => s.Progress());
        }
    }
}