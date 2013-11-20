using System.Collections.Generic;

namespace DrumScore
{
    public interface IPlaybackOutput
    {
        void PlayScoreStart(string creator);
        void Play(ICollection<Sample> samples);
        void PlayScoreEnd();
    }
}