using System.Collections.Generic;

namespace DrumScore
{
    public interface IScore
    {
        IList<IEnumerable<Sample>> Samples { get; set; }
        void AddSample(Sample sample);
        void Progress();
        void SetPosition(int i);
    }
}