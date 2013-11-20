using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DrumScore.Interpretation.Expressions;

namespace DrumScore.Interpretation
{
    public class Tokeniser
    {
        private const string samplePattern = @"^[\*\^o=]+$";
        private const string skipBeatPattern = ".";
        private const string beatSeparator = " ";

        public virtual IList<IExpression> ReadTokens(string score)
        {
            var expressions = new List<IExpression>();
            var tokens = score.Split(new[] { beatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, samplePattern)) expressions.Add(new SampleExpression(token));

                else if (OffsetSampleExpression.IsMatch(token)) expressions.Add(new OffsetSampleExpression(token));

                else if (token == skipBeatPattern) expressions.Add(new SkipBeatExpression());

                else throw new UnrecognisedTokenException(token);
            }

            return expressions;
        }
    }

    public class UnrecognisedTokenException : Exception
    {
        public UnrecognisedTokenException() { }
        public UnrecognisedTokenException(string message) : base(message) { }
    }
}
