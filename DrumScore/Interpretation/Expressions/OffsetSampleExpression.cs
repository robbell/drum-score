using System;
using System.Text.RegularExpressions;

namespace DrumScore.Interpretation.Expressions
{
    public class OffsetSampleExpression : IExpression
    {
        private const string offsetSamplePattern = @"^([0-8]?)(\\|\/)([\*\^o=]+$)";
        private readonly string token;

        public OffsetSampleExpression(string token)
        {
            this.token = token;
        }

        public void Interpret(IScore score)
        {
            var parts = GetParts();

            SetScorePosition(score, parts);

            var sampleExpression = new SampleExpression(parts[3].Value);

            sampleExpression.Interpret(score);
        }

        private GroupCollection GetParts()
        {
            return Regex.Match(token, offsetSamplePattern).Groups;
        }

        private void SetScorePosition(IScore score, GroupCollection parts)
        {
            var offset = string.IsNullOrEmpty(parts[1].Value) ? 1 : Convert.ToInt32(parts[1].Value);
            var timing = parts[2].Value == @"\" ? OffsetTiming.Early : OffsetTiming.Late;

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