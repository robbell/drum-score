namespace DrumScore.Expressions
{
    public interface IExpression
    {
        void Interpret(Score score);
    }
}