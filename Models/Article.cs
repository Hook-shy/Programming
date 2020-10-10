using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class Article : Model
    {
        public long UserId { get; set; }
        public string User { get; set; }
        public bool IsTop { get; set; } = false;
        public bool IsOriginal { get; set; }
        public bool IsPass { get; set; } = false;
        public DateTime PassDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string KeyWord { get; set; }
        public int LookCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public string Family { get; set; }
    }
}
