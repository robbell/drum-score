namespace DrumScore.Interpretation.Expressions
{
    public class SampleExpression : IExpression
    {
        public string Sample { get; set; }

        public SampleExpression(string sample)
        {
            Sample = sample;
        }

        public void Interpret(IScore score)
        {
            foreach (var key in Sample)
            {
                score.AddSample(new Sample(key.ToString()));
            }

            score.Progress();
        }
    }
}