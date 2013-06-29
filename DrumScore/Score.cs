using System.Collections.Generic;

namespace DrumScore
{
    public class Score : IScore
    {
        public IList<IEnumerable<Sample>> Samples { get; set; }

        public void AddSample(Sample sample)
        {
            throw new System.NotImplementedException();
        }

        public void Progress()
        {
            throw new System.NotImplementedException();
        }

        public void SetPosition(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}