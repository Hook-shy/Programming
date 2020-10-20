using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public enum QuestionType
    {
        SCQ = 0,
        MCQ = 1,
        TFQ = 2,
        CQ = 4
    }

    public class Question
    {
        public string Title { get; set;}
        public QuestionType type { get; set; }
        public string[] Options { get; set; }
        public int[] Answer { get; set; }
        public string Explain { get; set; }
        public string Source { get; set; }
    }
}
