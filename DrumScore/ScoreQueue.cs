using System.Collections.Generic;
using System.Linq;

namespace DrumScore
{
    public class ScoreQueue
    {
        private readonly IScoreFeed feed;
        private readonly Interpreter interpreter;
        private readonly INotifications notifications;

        public IList<ScoreInfo> Scores { get; private set; }

        public ScoreQueue(IScoreFeed feed, Interpreter interpreter, INotifications notifications)
        {
            this.feed = feed;
            this.interpreter = interpreter;
            this.notifications = notifications;
            Scores = new List<ScoreInfo>();
        }

        public void Update()
        {
            var latest = feed.GetLatest();

            foreach (var scoreInfo in latest.Where(item => !Scores.Any(s => s.Id == item.Id)))
            {
                try
                {
                    scoreInfo.Score = interpreter.Interpret(scoreInfo.TextScore);
                    Scores.Add(scoreInfo);
                }
                catch (UnrecognisedTokenException)
                {
                    notifications.SendError(scoreInfo);
                }
            }
        }
    }
}