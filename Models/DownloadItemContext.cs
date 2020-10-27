using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class DownloadItemContext : DbContext
    {
        public DownloadItemContext(DbContextOptions<DownloadItemContext> options) : base(options) { }

        public DbSet<DownloadItem> DownloadItems { get; set; }
        /*
         Add-Migration MyMigration -Context DownloadItemContext
         Update-Database -Context DownloadItemContext
         */
    }
}
