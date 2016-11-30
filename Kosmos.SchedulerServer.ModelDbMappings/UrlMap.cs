using Kosmos.SchedulerServer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmos.SchedulerServer.ModelDbMappings {
    public class UrlMap : EntityTypeConfiguration<Url> {
        public UrlMap() {
            this.HasKey(url => url.HashCode);
            this.Property(url => url.HashCode)
                .HasMaxLength(32);

            this.Property(url => url.Depth)
                .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(new IndexAttribute("Depth")));
        }
    }
}
