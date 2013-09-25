using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using Tweetinvi;
using TwitterToken;
using System.Linq;

namespace DrumScore.ScoreSourcing
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
            try
            {
                var user = new TokenUser(token);

                return user.GetLatestMentionsTimeline().Select(m => new ScoreInfo
                {
                    Id = m.Id,
                    TextScore = StripMentions(m.Text),
                    Username = m.InReplyToScreenName,
                    DateTime = m.CreatedAt
                }).ToList();

            }
            catch (WebException)
            {
                return Enumerable.Empty<ScoreInfo>().ToList();
            }
        }

        private string StripMentions(string tweet) // ToDo: this should be unit tested
        {
            return Regex.Replace(tweet, @"@([A-Za-z0-9_]+\w)", string.Empty);
        }
    }
}