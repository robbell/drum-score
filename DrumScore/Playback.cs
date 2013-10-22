using System.Diagnostics;

namespace DrumScore
{
    public class Playback
    {
        private readonly IPlaybackOutput output;
        private const int millisecondsBetweenFrames = 100;

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public void Play(IScore score)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var position = 0; position < score.Samples.Count; position++)
            {
                WaitForNextFrame(stopwatch);
                output.Play(score.Samples[position]);
                stopwatch.Restart();
            }
        }

        private void WaitForNextFrame(Stopwatch stopwatch)
        {
            while (stopwatch.ElapsedMilliseconds < millisecondsBetweenFrames) { }
        }
    }
}