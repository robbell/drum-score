namespace DrumScore
{
    public class Playback
    {
        private readonly IPlaybackOutput output;

        public Playback(IPlaybackOutput output)
        {
            this.output = output;
        }

        public void Play(IScore score)
        {
            for (var position = 0; position < score.Samples.Count; position++)
            {
                output.Play(score.Samples[position]);
            }
        }
    }
}