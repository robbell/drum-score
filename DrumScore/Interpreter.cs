namespace DrumScore
{
    public class Interpreter
    {
        private readonly Tokeniser tokeniser;

        public Interpreter(Tokeniser tokeniser)
        {
            this.tokeniser = tokeniser;
        }

        public Score Interpret(string textScore)
        {
            var score = new Score();
            var tokens = tokeniser.ReadTokens(textScore);

            foreach (var token in tokens)
            {
                token.Interpret(score);
            }

            return score;
        }
    }
}