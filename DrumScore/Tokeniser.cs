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
        private const char beatSeparator = ' ';

        public IList<IExpression> ReadTokens(string score)
        {
            var expressions = new List<IExpression>();
            var beats = score.Split(new[] { beatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var beat in beats)
            {
                if (Regex.IsMatch(beat, samplePattern)) expressions.Add(new SampleExpression(beat));

                if (Regex.IsMatch(beat, offsetSamplePattern)) expressions.Add(CreateOffsetExpression(beat));

                if (beat == skipBeatPattern) expressions.Add(new SkipBeatExpression());
            }

            return expressions;
        }

        private OffsetSampleExpression CreateOffsetExpression(string beat)
        {
            var groups = Regex.Match(beat, offsetSamplePattern).Groups;

            var offset = string.IsNullOrEmpty(groups[1].Value) ? 1 : Convert.ToInt32(groups[1].Value);
            var timing = groups[2].Value == @"\" ? OffsetTiming.Early : OffsetTiming.Late;

            var expression = new OffsetSampleExpression { Timing = timing, Offset = offset };
            return expression;
        }
    }
}
