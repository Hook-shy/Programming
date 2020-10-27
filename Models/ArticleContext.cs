using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Models
{
    public class ArticleContext : DbContext
    {
        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        /*
         Add-Migration MyMigration -Context ArticleContext
         Update-Database -Context ArticleContext
         */
    }
}
