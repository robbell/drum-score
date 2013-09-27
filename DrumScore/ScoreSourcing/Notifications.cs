using System.Net;
using DrumScore.Interpretation;
using Tweetinvi;

namespace DrumScore.ScoreSourcing
{
    public class Notifications : INotifications
    {
        private const int maxLength = 140;

        public void SendError(ScoreInfo invalidScore, UnrecognisedTokenException exception)
        {
            try
            {
                var user = new TokenUser(TokenSingleton.Token);

                var tweet = string.Format("@{0} Unrecognised token: \"{1}\".",
                                          invalidScore.Username,
                                          exception.Message);

                user.SendTweet(tweet.Length > maxLength ? tweet.Substring(0, maxLength) : tweet);
            }
            catch (WebException)
            {
                // ToDo: implement UI notifications
            }
        }
    }
}