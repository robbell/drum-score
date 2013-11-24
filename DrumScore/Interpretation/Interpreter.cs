using DrumScore.Interpretation.Expressions;

namespace DrumScore.Interpretation
{
    public class Interpreter
    {
        private readonly Tokeniser tokeniser;

        public Interpreter(Tokeniser tokeniser)
        {
            this.tokeniser = tokeniser;
        }

        public virtual Score Interpret(string textScore)
        {
            var score = new Score();
            var tokens = tokeniser.ReadTokens(textScore);

            AddEmptyFirstBeat(score);

            foreach (var token in tokens)
            {
                token.Interpret(score);
            }

            return score;
        }

        private void AddEmptyFirstBeat(IScore score)
        {
            new SkipBeatExpression().Interpret(score);
        }
    }
}