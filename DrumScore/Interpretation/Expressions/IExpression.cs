namespace DrumScore.Interpretation.Expressions
{
    public interface IExpression
    {
        void Interpret(IScore score);
    }
}