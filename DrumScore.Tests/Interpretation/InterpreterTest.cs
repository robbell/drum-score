using DrumScore.Interpretation;
using DrumScore.Interpretation.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests.Interpretation
{
    [TestFixture]
    public class InterpreterTest
    {
        [Test]
        public void InterpreterTokenisesAndInterpretsExpressionString()
        {
            const string textScore = "*^  o = =";

            var firstExpression = new Mock<IExpression>();
            var lastExpression = new Mock<IExpression>();

            var expressions = new[] { firstExpression.Object, lastExpression.Object };

            var tokeniser = new Mock<Tokeniser>();
            tokeniser.Setup(t => t.ReadTokens(textScore)).Returns(expressions);

            new Interpreter(tokeniser.Object).Interpret(textScore);

            firstExpression.Verify(e => e.Interpret(It.IsAny<IScore>()));
            lastExpression.Verify(e => e.Interpret(It.IsAny<IScore>()));
        }
    }
}
