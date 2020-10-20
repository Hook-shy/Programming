using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public enum ScoreType
    {
        MAX = 0,
        MIN = 1,
        AVERAGE = 2
    }

    public class MatchConf : Model
    {
        public string Name { get; set; }
        public DateTime PracticeStartTime { get; set; }
        public DateTime PracticeEndTime { get; set; }
        public DateTime MatchStartTime { get; set; }
        public DateTime MatchEndTime { get; set; }
        public string MainColor { get; set; } = "#08FFC8";
        public string InfoTab1Title { get; set; } = "比赛规则";
        public string InfoTab1Content { get; set; }
        public string InfoTab2Title { get; set; } = "比赛背景";
        public string InfoTab2Content { get; set; }
        public string InfoTab3Title { get; set; } = "比赛目的";
        public string InfoTab3Content { get; set; }
        public DateTime ShowRankingStartTime { get; set; }
        public DateTime ShowRankingEndTime { get; set; }
        public int SCQCount { get; set; } = 20;
        public int MCQCount { get; set; } = 6;
        public int TFQCount { get; set; } = 20;
        public int CQCount { get; set; } = 4;
        public int SCQPoint { get; set; } = 2;
        public int MCQPoint { get; set; } = 2;
        public int TFQPoint { get; set; } = 2;
        public int CQPoint { get; set; } = 2;
        public int MatchTimes { get; set; } = 3;
        public ScoreType scoreType { get; set; } = ScoreType.MAX;
    }
}
