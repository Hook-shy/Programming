using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class SysMsgContext : DbContext
    {
        public SysMsgContext(DbContextOptions<SysMsgContext> options) : base(options) { }

        public DbSet<SysMsg> UserMessages { get; set; }
        /*
         Add-Migration MyMigration -Context UserMessageContext
         Update-Database -Context UserMessageContext
         */
    }
}
