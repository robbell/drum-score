﻿namespace DrumScore.Expressions
{
    public class SkipBeatExpression : IExpression
    {
        public void Interpret(IScore score)
        {
            score.Progress();
        }
    }
}