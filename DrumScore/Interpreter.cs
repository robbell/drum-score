namespace DrumScore
{
    public class Interpreter
    {
        private readonly Tokeniser tokeniser;

        public Interpreter(Tokeniser tokeniser)
        {
            this.tokeniser = tokeniser;
        }

        public Score Interpret(string twitterScore)
        {
            var score = new Score();
            var tokens = tokeniser.ReadTokens(twitterScore);

            foreach (var token in tokens)
            {
                token.Interpret(score);
            }

            return score;
        }
    }
}