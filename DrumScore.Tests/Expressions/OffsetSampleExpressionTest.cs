﻿using DrumScore.Expressions;
using Moq;
using NUnit.Framework;

namespace DrumScore.Tests.Expressions
{
    [TestFixture]
    public class OffsetSampleExpressionTest
    {
        [Test]
        public void EarlyExpressionMovesScorePosition()
        {
            const int offset = 2;
            var score = new Mock<IScore>();
            var expression = new OffsetSampleExpression("*") { Offset = offset, Timing = OffsetTiming.Early };

            expression.Interpret(score.Object);

            score.Verify(s => s.SetPosition(-offset));
            score.Verify(s => s.AddSample(It.IsAny<Sample>()), Times.Once());
            score.Verify(s => s.Progress());
        }
    }
}