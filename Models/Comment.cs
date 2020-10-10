using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class Comment : Model
    {
        public long UserId { get; set; }
        public long ArticleId { get; set; }
        public string Content { get; set; }
        public int StarCount { get; set; } = 0;
    }
}
