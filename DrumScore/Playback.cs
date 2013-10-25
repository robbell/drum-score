using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace DrumScore
{
    public class Playback
    {
        private const int millisecondsBetweenFrames = 30;
        private readonly IPlaybackOutput output;
        public virtual event PlaybackComplete Complete;

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public virtual void Play(IScore score)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) => BeginPlay(score);
            worker.RunWorkerCompleted += (s, e) => OnComplete();
            worker.RunWorkerAsync();
        }

        private void BeginPlay(IScore score)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var position = 0; position <= score.Samples.Keys.Last(); position++)
            {
                WaitForNextFrame(stopwatch);

                if (score.Samples.ContainsKey(position)) output.Play(score.Samples[position]);

                stopwatch.Restart();
            }
        }

        private void WaitForNextFrame(Stopwatch stopwatch)
        {
            while (stopwatch.ElapsedMilliseconds < millisecondsBetweenFrames) { }
        }

        private void OnComplete()
        {
            if (Complete != null) Complete();
        }
    }

    public delegate void PlaybackComplete();
}