using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace DrumScore
{
    public class Playback
    {
        public virtual event PlaybackComplete Complete;
        private readonly int millisecondsBetweenFrames = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenFrames"]);
        private readonly IPlaybackOutput output;

        public bool IsPlaying { get; private set; }

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public virtual void Play(ScoreInfo info)
        {
            IsPlaying = true;

            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) => BeginPlay(info);
            worker.RunWorkerCompleted += (s, e) => OnComplete();
            worker.RunWorkerAsync();
        }

        private void BeginPlay(ScoreInfo info)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            output.PlayScoreStart(info.Username);

            PlayScore(info.Score, stopwatch);

            output.PlayScoreEnd();
        }

        private void PlayScore(IScore score, Stopwatch stopwatch)
        {
            for (var position = 0; position <= score.Samples.Keys.Last(); position++)
            {
                Wait(stopwatch, millisecondsBetweenFrames);

                output.Play(score.Samples.ContainsKey(position) ? score.Samples[position] : new Collection<Sample>());

                stopwatch.Restart();
            }
        }

        private void OnComplete()
        {
            IsPlaying = false;

            if (Complete != null) Complete();
        }

        private void Wait(Stopwatch stopwatch, int millisecondsToWait)
        {
            while (stopwatch.ElapsedMilliseconds < millisecondsToWait) { }
        }
    }

    public delegate void PlaybackComplete();
}