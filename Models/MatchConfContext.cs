using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class MatchConfContext : DbContext
    {
        public MatchConfContext(DbContextOptions<MatchConfContext> options) : base(options) { }

        public DbSet<MatchConf> MathConfs { get; set; }

        /*
         Add-Migration MyMigration -Context MathConfContext
         Update-Database -Context MathConfContext
         */
    }
}
