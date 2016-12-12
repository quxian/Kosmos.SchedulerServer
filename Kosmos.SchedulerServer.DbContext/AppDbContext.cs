using Kosmos.SchedulerServer.Model;
using Kosmos.SchedulerServer.ModelDbMappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.SchedulerServer.DbContext {
    public class AppDbContext : System.Data.Entity.DbContext {
        public AppDbContext() : base("SchedulerServerDbConnection") { }

        public DbSet<Url> Urls { get; set; }
        public DbSet<DownloadErrorUrl> DownloadErrorUrls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new UrlMap());
            modelBuilder.Configurations.Add(new DownloadErrorUrlMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
