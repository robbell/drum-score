using DrumScore.Interpretation;

namespace DrumScore.ScoreSourcing
{
    public interface INotifications
    {
        void SendError(ScoreInfo invalidScore, UnrecognisedTokenException exception);
    }
}