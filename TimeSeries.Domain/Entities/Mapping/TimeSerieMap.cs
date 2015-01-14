using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TimeSeries.Domain.Entities
{
    public class TimeSerieMap : EntityTypeConfiguration<TimeSerie>
    {
        public TimeSerieMap()
        {
            // Primary Key
            this.HasKey(t => t.TimeSerieId);

            // Properties
            this.Property(t => t.User_Id)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TimeSeries");
            this.Property(t => t.TimeSerieId).HasColumnName("TimeSerieId");
            this.Property(t => t.VectorData).HasColumnName("VectorData");
            this.Property(t => t.User_Id).HasColumnName("User_Id");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.TimeSeries)
                .HasForeignKey(d => d.User_Id);
        }
    }
}
