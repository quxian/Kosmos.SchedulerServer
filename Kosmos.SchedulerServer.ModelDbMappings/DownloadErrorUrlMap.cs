using Kosmos.SchedulerServer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.SchedulerServer.ModelDbMappings
{
    public class DownloadErrorUrlMap : EntityTypeConfiguration<DownloadErrorUrl>
    {
        public DownloadErrorUrlMap()
        {
            this.HasKey(downloadErrorUrl => downloadErrorUrl.UrlHashCode);
            this.Property(downloadErrorUrl => downloadErrorUrl.UrlHashCode)
                .HasMaxLength(32);
        }
    }
}
