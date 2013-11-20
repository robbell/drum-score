using DrumScore.ScoreSourcing;

namespace DrumScore
{
    public class PlaybackQueue
    {
        private readonly ScoreQueue scoreQueue;
        private readonly Playback playback;
        public event PlaybackComplete QueueComplete;

        public bool PullFromScoreQueue { get; set; }

        public PlaybackQueue(ScoreQueue scoreQueue, Playback playback)
        {
            this.scoreQueue = scoreQueue;
            this.playback = playback;
            playback.Complete += OnScoreComplete;
        }

        public void Play()
        {
            PlayNext();
        }

        private void OnScoreComplete()
        {
            PlayNext();
        }

        private void PlayNext()
        {
            var scoreInfo = scoreQueue.GetNextScoreToPlay();

            if (scoreInfo == null && PullFromScoreQueue)
            {
                scoreQueue.MoveTopTweetToPlaylist();
                scoreInfo = scoreQueue.GetNextScoreToPlay();
            }

            if (scoreInfo != null) playback.Play(scoreInfo);
            else if (QueueComplete != null) QueueComplete();
        }
    }
}