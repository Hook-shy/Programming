using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class SysMsg : Model
    {
        public long UserId { get; set; }

        public DateTime ReadTime { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
}

