using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class Competition : Model
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Finish { get; set; }
        public long MatchId { get; set; }
        public int Score { get; set; }
        public Question[] questions { get; set; }
        public int[,] Answer { get; set; }
    }
}
