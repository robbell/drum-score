using System.Collections.Generic;
using System.Linq;

namespace DrumScore
{
    public class Score : IScore
    {
        private const int progression = 8;
        private int position;
        private int offset;

        public IDictionary<int, ICollection<Sample>> Samples { get; private set; }

        public Score()
        {
            Samples = new Dictionary<int, ICollection<Sample>>();
        }

        public void AddSample(Sample sample)
        {
            var offsetPosition = position + offset;

            if (offsetPosition < 0) offsetPosition = 0;

            if (!Samples.ContainsKey(offsetPosition)) Samples.Add(offsetPosition, new List<Sample>());

            if (Samples[offsetPosition].All(s => s.Type != sample.Type)) Samples[offsetPosition].Add(sample);
        }

        public void SetPosition(int value)
        {
            offset = value;
        }

        public void Progress()
        {
            position += progression;
            offset = 0;
        }
    }
}