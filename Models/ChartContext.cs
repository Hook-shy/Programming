using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class ChartContext : DbContext
    {
        public ChartContext(DbContextOptions<ChartContext> options) : base(options)
        {

        }

        public DbSet<Chart> Charts { get; set; }
        /*
         Add-Migration MyMigration -Context UserContext
         Update-Database -Context UserContext
         */
    }
}
