using System;

namespace DrumScore
{
    public class ScoreInfo
    {
        public long? Id { get; set; }
        public string Username { get; set; }
        public string TextScore { get; set; }
        public Score Score { get; set; }
        public DateTime DateTime { get; set; }
    }
}