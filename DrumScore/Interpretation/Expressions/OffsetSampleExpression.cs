namespace DrumScore.Interpretation.Expressions
{
    public class OffsetSampleExpression : IExpression
    {
        public int Offset { get; set; }
        public OffsetTiming Timing { get; set; }
        private readonly SampleExpression sampleExpression;

        public OffsetSampleExpression(string sample)
        {
            sampleExpression = new SampleExpression(sample);
            Offset = 1;
        }

        public void Interpret(IScore score)
        {
            score.SetPosition(Timing == OffsetTiming.Early ? -Offset : Offset);
            sampleExpression.Interpret(score);
        }
    }

    public enum OffsetTiming
    {
        Early,
        Late
    }
}