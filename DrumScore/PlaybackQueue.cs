using DrumScore.ScoreSourcing;

namespace DrumScore
{
    public class PlaybackQueue
    {
        private readonly ScoreQueue scoreQueue;
        private readonly Playback playback;

        public PlaybackQueue(ScoreQueue scoreQueue, Playback playback)
        {
            this.scoreQueue = scoreQueue;
            this.playback = playback;
        }

        public void Play()
        {
            var scoreInfo = scoreQueue.GetNextScoreToPlay();
            playback.Play(scoreInfo.Score);
        }
    }
}