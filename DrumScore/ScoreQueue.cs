using System.Collections.Generic;
using System.Linq;

namespace DrumScore
{
    public class ScoreQueue
    {
        private readonly IScoreReader reader;

        public IList<ScoreInfo> Items { get; private set; }

        public ScoreQueue(IScoreReader reader)
        {
            this.reader = reader;
            Items = new List<ScoreInfo>();
        }

        public void Update()
        {
            var latest = reader.GetLatest();

            foreach (var item in latest.Where(item => !Items.Any(s => s.Id == item.Id)))
            {
                Items.Add(item);
            }
        }
    }
}