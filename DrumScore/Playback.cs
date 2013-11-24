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
        private readonly IPlaybackOutput output;
        private int millisecondsBetweenFrames;

        public bool IsPlaying { get; private set; }

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public virtual void Play(ScoreInfo info, int tempo = 60)
        {
            SetTimeBetweenFrames(tempo);

            IsPlaying = true;

            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) => BeginPlay(info);
            worker.RunWorkerCompleted += (s, e) => OnComplete();
            worker.RunWorkerAsync();
        }

        private void SetTimeBetweenFrames(int tempo)
        {
            const int framesPerBeat = 32;
            var beatsPerSecond = tempo / 60d;
            millisecondsBetweenFrames = (int)(1000 / (framesPerBeat * beatsPerSecond));
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