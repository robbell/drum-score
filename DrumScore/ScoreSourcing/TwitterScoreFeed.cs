using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using Streaminvi;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using Tweetinvi;
using TwitterToken;

namespace DrumScore.ScoreSourcing
{
    public class TwitterScoreFeed : IScoreFeed
    {
        public event TweetReceived Received;
        private readonly Token token;

        public TwitterScoreFeed()
        {
            token = new Token(ConfigurationManager.AppSettings["AccessToken"],
                              ConfigurationManager.AppSettings["AccessToken.Secret"],
                              ConfigurationManager.AppSettings["Consumer.Key"],
                              ConfigurationManager.AppSettings["Consumer.Secret"]);

            TokenSingleton.Token = token;
        }

        public void StartListening()
        {
            var worker = new BackgroundWorker();

            worker.DoWork += (s, e) =>
                {
                    var stream = new UserStream(token);
                    stream.TweetCreatedByAnyoneButMe += (a, b) => worker.ReportProgress(0, b);
                    stream.StartStream();
                };

            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += (s, e) => TweetReceived((GenericEventArgs<ITweet>)e.UserState);
            worker.RunWorkerAsync();
        }

        private void TweetReceived(GenericEventArgs<ITweet> tweetEvent)
        {
            if (!tweetEvent.Value.Text.Contains(ConfigurationManager.AppSettings["AccountName"])) return;

            var tweet = tweetEvent.Value;

            var scoreInfo = new ScoreInfo
                {
                    Id = tweet.Id,
                    TextScore = StripMentions(tweet.Text),
                    Username = tweet.Creator.ScreenName,
                    DateTime = tweet.CreatedAt,
                };

            if (Received != null) Received(scoreInfo);
        }

        private string StripMentions(string tweet) // ToDo: this should be unit tested
        {
            return Regex.Replace(tweet, @"@([A-Za-z0-9_]+\w)", string.Empty);
        }

        private string DecodeBackslashes(string tweet) // ToDo: this should be unit tested
        {
            return tweet.Replace(@"\\", @"\");
        }
    }

    public delegate void TweetReceived(ScoreInfo scoreInfo);
}