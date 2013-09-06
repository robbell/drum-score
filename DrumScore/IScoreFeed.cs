using System.Collections.Generic;

namespace DrumScore
{
    public interface IScoreFeed
    {
        IList<ScoreInfo> GetLatest();
    }
}