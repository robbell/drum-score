using System;
using DrumScore.Interpretation;
using Tweetinvi;

namespace DrumScore.ScoreSourcing
{
    public class Notifications : INotifications
    {
        private const int maxLength = 130;

        public void SendError(ScoreInfo invalidScore, UnrecognisedTokenException exception)
        {
            try
            {
                var user = new TokenUser(TokenSingleton.Token);

                var tweet = string.Format("@{0} Unrecognised token: \"{1}\". Language spec: http://t.co/mESKiN9qSS",
                                          invalidScore.Username,
                                          exception.Message);

                user.PublishTweet(tweet.Length > maxLength ? tweet.Substring(0, maxLength) : tweet);
            }
            catch (Exception)
            {
                // ToDo: implement UI notifications
            }
        }
    }
}