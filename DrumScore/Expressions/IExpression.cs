namespace DrumScore.Expressions
{
    public interface IExpression
    {
        void Interpret(IScore score);
    }
}