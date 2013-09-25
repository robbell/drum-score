namespace DrumScore.ScoreSourcing
{
    public interface INotifications
    {
        void SendError(ScoreInfo invalidScore);
    }
}