using DrumScore.ScoreSourcing;

namespace DrumScore
{
    public class PlaybackQueue
    {
        private readonly ScoreQueue scoreQueue;
        private readonly Playback playback;
        public event PlaybackComplete Complete;

        public PlaybackQueue(ScoreQueue scoreQueue, Playback playback)
        {
            this.scoreQueue = scoreQueue;
            this.playback = playback;
            playback.Complete += OnPlaybackComplete;
        }

        public void Play()
        {
            PlayNext();
        }

        private void OnPlaybackComplete()
        {
            PlayNext();
        }

        private void PlayNext()
        {
            var scoreInfo = scoreQueue.GetNextScoreToPlay();

            if (scoreInfo != null) playback.Play(scoreInfo.Score);
            else if (Complete != null) Complete();
        }
    }
}