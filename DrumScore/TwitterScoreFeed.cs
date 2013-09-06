using System.Collections.Generic;
using System.Configuration;
using Tweetinvi;
using TwitterToken;
using System.Linq;

namespace DrumScore
{
    public class TwitterScoreFeed : IScoreFeed
    {
        private readonly Token token;

        public TwitterScoreFeed()
        {
            token = new Token(ConfigurationManager.AppSettings["AccessToken"],
                              ConfigurationManager.AppSettings["AccessToken.Secret"],
                              ConfigurationManager.AppSettings["Consumer.Key"],
                              ConfigurationManager.AppSettings["Consumer.Secret"]);

            TokenSingleton.Token = token;
        }

        public IList<ScoreInfo> GetLatest()
        {
            var user = new TokenUser(token);

            return user.GetLatestMentionsTimeline().Select(m => new ScoreInfo
                {
                    Id = m.Id,
                    TextScore = m.Text,
                    Username = m.InReplyToScreenName
                }).ToList();
        }
    }
}