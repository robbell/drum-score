using System.Net;
using DrumScore.Interpretation;
using Tweetinvi;

namespace DrumScore.ScoreSourcing
{
    public class Notifications : INotifications
    {
        public void SendError(ScoreInfo invalidScore, UnrecognisedTokenException exception)
        {
            try
            {
                var user = new TokenUser(TokenSingleton.Token);

                var tweet = string.Format("@{0} Unrecognised token: \"{1}\". Full details: {2}",
                                          invalidScore.Username,
                                          exception.Message,
                                          exception)
                                  .Substring(0, 140);

                user.SendTweet(tweet);
            }
            catch (WebException)
            {
                // ToDo: implement UI notifications
            }
        }
    }
}