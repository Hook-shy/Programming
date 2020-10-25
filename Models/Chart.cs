using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public enum ChartType
    {
        HOME = 0,
        MATCH = 1
    }

    public class Chart: Model
    {
        public string Title { get; set; }
        public string BigUrl { get; set; }
        public string SmallUrl { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MatchId { get; set; }
        public ChartType ChartType { get; set; }
        public bool Enable { get; set; }
    }
}
