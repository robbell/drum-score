using System.Collections.Generic;

namespace DrumScore.ScoreSourcing
{
    public interface IScoreFeed
    {
        IList<ScoreInfo> GetLatest();
    }
}