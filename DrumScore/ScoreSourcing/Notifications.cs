using System;
using System.Configuration;
using DrumScore.Interpretation;
using Tweetinvi;

namespace DrumScore.ScoreSourcing
{
    public class Notifications : INotifications
    {
        private const int tokenLength = 55;

        public void SendError(ScoreInfo invalidScore, UnrecognisedTokenException exception)
        {
            try
            {
                var sendNotifications = bool.Parse(ConfigurationManager.AppSettings["SendNotifications"]);

                if (!sendNotifications) return;

                var user = new TokenUser(TokenSingleton.Token);

                var tweet = string.Format("@{0} Unrecognised token: \"{1}\". Language spec: http://t.co/mESKiN9qSS",
                                          invalidScore.Username,
                                          exception.Message.Length > tokenLength ? exception.Message.Substring(0, tokenLength) : exception.Message);

                user.PublishTweet(tweet);
            }
            catch (Exception)
            {
                // ToDo: implement UI notifications
            }
        }
    }
}