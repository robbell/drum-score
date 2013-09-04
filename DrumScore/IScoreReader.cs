using System.Collections.Generic;

namespace DrumScore
{
    public interface IScoreReader
    {
        IList<ScoreInfo> GetLatest();
    }
}