using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace DrumScore.Interpretation.Expressions
{
    public class OffsetSampleExpression : IExpression
    {
        private const string offsetSamplePattern = @"^(([0-7])(\\|\/)|\\{1,7}|\/{1,7})([1-4]+$)";
        private readonly string token;

        public OffsetSampleExpression(string token)
        {
            this.token = token;
        }

        public void Interpret(IScore score)
        {
            var parts = GetParts();

            SetScorePosition(score, parts);

            var sampleExpression = new SampleExpression(parts[4].Value);

            sampleExpression.Interpret(score);
        }

        private GroupCollection GetParts()
        {
            return Regex.Match(token, offsetSamplePattern).Groups;
        }

        private void SetScorePosition(IScore score, GroupCollection parts)
        {
            var offset = string.IsNullOrEmpty(parts[2].Value) ? parts[1].Length : Convert.ToInt32(parts[2].Value);
            var timing = parts[1].Value.Last() == '\\' ? OffsetTiming.Early : OffsetTiming.Late;

            score.SetPosition(timing == OffsetTiming.Early ? -offset : offset);
        }

        public static bool IsMatch(string token)
        {
            return Regex.IsMatch(token, offsetSamplePattern);
        }
    }

    public enum OffsetTiming
    {
        Early,
        Late
    }
}