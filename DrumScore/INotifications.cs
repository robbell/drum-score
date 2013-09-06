namespace DrumScore
{
    public interface INotifications
    {
        void SendError(ScoreInfo invalidScore);
    }
}