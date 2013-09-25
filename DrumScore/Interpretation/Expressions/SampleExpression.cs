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
            var bank = new SampleBank(); // ToDo: Should be injected via IoC

            foreach (var key in Sample)
            {
                score.AddSample(bank.Load(key.ToString()));
            }

            score.Progress();
        }
    }

    public class SampleBank
    {
        public Sample Load(string key)
        {
            return new Sample(key);
        }
    }
}