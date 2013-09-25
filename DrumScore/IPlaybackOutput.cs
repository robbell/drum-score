using System.Collections.Generic;

namespace DrumScore
{
    public interface IPlaybackOutput
    {
        void Play(ICollection<Sample> samples);
    }
}