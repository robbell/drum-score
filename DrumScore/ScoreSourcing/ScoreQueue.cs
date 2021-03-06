﻿using System.Collections.Generic;
using System.Linq;
using DrumScore.Interpretation;

namespace DrumScore.ScoreSourcing
{
    public class ScoreQueue
    {
        private readonly IScoreFeed feed;
        private readonly Interpreter interpreter;
        private readonly INotifications notifications;

        public IList<ScoreInfo> Tweets { get; private set; }
        public IList<ScoreInfo> Playlist { get; private set; }
        public event QueueChanged QueueChanged;

        public ScoreQueue(IScoreFeed feed, Interpreter interpreter, INotifications notifications)
        {
            this.feed = feed;
            this.interpreter = interpreter;
            this.notifications = notifications;
            Tweets = new List<ScoreInfo>();
            Playlist = new List<ScoreInfo>();
            feed.Received += ScoreReceived;
        }

        public void StartListening()
        {
            feed.StartListening();
        }

        public void ScoreReceived(ScoreInfo scoreInfo)
        {
            try
            {
                scoreInfo.Score = interpreter.Interpret(scoreInfo.TextScore);
                Tweets.Add(scoreInfo);
                OnQueueChanged();
            }
            catch (UnrecognisedTokenException exception)
            {
                notifications.SendError(scoreInfo, exception);
            }
        }

        public void MoveToPlaylist(ScoreInfo scoreToMove)
        {
            Tweets.Remove(scoreToMove);
            Playlist.Add(scoreToMove);
        }

        public void MoveItemUp(ScoreInfo scoreToMove)
        {
            var itemIndex = Playlist.IndexOf(scoreToMove);

            if (itemIndex <= 0) return;

            Playlist.RemoveAt(itemIndex);
            Playlist.Insert(itemIndex - 1, scoreToMove);
        }

        public void MoveItemDown(ScoreInfo scoreToMove)
        {
            var itemIndex = Playlist.IndexOf(scoreToMove);

            if (itemIndex == Playlist.Count - 1) return;

            Playlist.RemoveAt(itemIndex);
            Playlist.Insert(itemIndex + 1, scoreToMove);
        }

        public virtual ScoreInfo GetNextScoreToPlay()
        {
            var nextScore = Playlist.FirstOrDefault();

            if (nextScore == null) MoveTopTweetToPlaylist();

            nextScore = Playlist.FirstOrDefault();

            if (nextScore == null) return null;

            Playlist.Remove(nextScore);
            OnQueueChanged();

            return nextScore;
        }

        private void OnQueueChanged()
        {
            if (QueueChanged != null) QueueChanged();
        }

        public virtual void MoveTopTweetToPlaylist()
        {
            if (Tweets.Any()) MoveToPlaylist(Tweets.First());
        }
    }

    public delegate void QueueChanged();
}