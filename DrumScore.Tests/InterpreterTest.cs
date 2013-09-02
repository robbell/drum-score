using DrumScore.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests
{
    [TestFixture]
    public class InterpreterTest
    {
        [Test]
        public void InterpreterTokenisesAndInterpretsExpressionString()
        {
            const string twitterScore = "*^  o = =";

            var firstExpression = new Mock<IExpression>();
            var lastExpression = new Mock<IExpression>();

            var expressions = new[] { firstExpression.Object, lastExpression.Object };

            var tokeniser = new Mock<Tokeniser>();
            tokeniser.Setup(t => t.ReadTokens(twitterScore)).Returns(expressions);

            new Interpreter(tokeniser.Object).Interpret(twitterScore);

            firstExpression.Verify(e => e.Interpret(It.IsAny<IScore>()));
            lastExpression.Verify(e => e.Interpret(It.IsAny<IScore>()));
        }
    }
}
