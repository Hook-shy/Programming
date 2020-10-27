using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class QuestionContext : DbContext
    {
        public QuestionContext(DbContextOptions<QuestionContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        /*
         Add-Migration MyMigration -Context QuestionContext
         Update-Database -Context QuestionContext
         */
    }
}
