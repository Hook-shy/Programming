using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class Model
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
