namespace DrumScore.ScoreSourcing
{
    public interface IScoreFeed
    {
        event TweetReceived Received;
        void StartListening();
    }
}