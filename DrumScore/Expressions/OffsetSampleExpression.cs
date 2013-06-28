using System;

namespace DrumScore.Expressions
{
    public class OffsetSampleExpression : IExpression
    {
        public int Offset { get; set; }
        public OffsetTiming Timing { get; set; }

        public OffsetSampleExpression()
        {
            Offset = 1;
        }

        public void Interpret(IScore score)
        {
            throw new NotImplementedException();
        }
    }

    public enum OffsetTiming
    {
        Early,
        Late
    }
}