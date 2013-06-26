namespace DrumScore.Expressions
{
    public class SampleExpression : IExpression
    {
        public string Sample { get; set; }

        public SampleExpression(string sample)
        {
            Sample = sample;
        }

        public void Interpret(Score score)
        {
            throw new System.NotImplementedException();
        }
    }
}