using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DrumScore.Expressions;

namespace DrumScore
{
    public class Tokeniser
    {
        private const string samplePattern = @"^[\*\^o=]+$";
        private const string offsetSamplePattern = @"^([0-8]?)(\\|\/)([\*\^o=]+$)";
        private const string skipBeatPattern = ".";
        private const string beatSeparator = " ";

        public IList<IExpression> ReadTokens(string score)
        {
            var expressions = new List<IExpression>();
            var tokens = score.Split(new[] { beatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, samplePattern)) expressions.Add(new SampleExpression(token));

                else if (Regex.IsMatch(token, offsetSamplePattern)) expressions.Add(CreateOffsetExpression(token));

                else if (token == skipBeatPattern) expressions.Add(new SkipBeatExpression());

                else throw new UnrecognisedTokenException(token);
            }

            return expressions;
        }

        private OffsetSampleExpression CreateOffsetExpression(string beat)
        {
            var groups = Regex.Match(beat, offsetSamplePattern).Groups;

            var offset = string.IsNullOrEmpty(groups[1].Value) ? 1 : Convert.ToInt32(groups[1].Value);
            var timing = groups[2].Value == @"\" ? OffsetTiming.Early : OffsetTiming.Late;

            return new OffsetSampleExpression { Timing = timing, Offset = offset };
        }
    }

    public class UnrecognisedTokenException : Exception
    {
        public UnrecognisedTokenException(string message) : base(message) { }
    }
}
