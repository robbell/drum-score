using System.Collections.Generic;

namespace DrumScore
{
    public interface IScore
    {
        IDictionary<int, ICollection<Sample>> Samples { get; }
        void AddSample(Sample sample);
        void Progress();
        void SetPosition(int value);
    }
}