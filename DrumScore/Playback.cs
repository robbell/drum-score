﻿using System.Diagnostics;

namespace DrumScore
{
    public class Playback
    {
        private const int millisecondsBetweenFrames = 100;
        private readonly IPlaybackOutput output;
        public virtual event PlaybackComplete Complete;

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public virtual void Play(IScore score)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var position = 0; position < score.Samples.Count; position++)
            {
                WaitForNextFrame(stopwatch);
                output.Play(score.Samples[position]);
                stopwatch.Restart();
            }

            OnComplete();
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