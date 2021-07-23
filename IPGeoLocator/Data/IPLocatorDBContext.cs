using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IPGeoLocator.Models
{
    public partial class IPLocatorDBContext : DbContext
    {
        public IPLocatorDBContext()
        {
        }

        public IPLocatorDBContext(DbContextOptions<IPLocatorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Ipbatch> Ipbatches { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.BatchId)
                    .ValueGeneratedNever()
                    .HasColumnName("BatchID");
                entity.ToTable("Batches");
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Ipbatch>(entity =>
            {
                entity.HasKey(e => e.InternalCode);

                entity.ToTable("IPBatch");

                entity.Property(e => e.BatchId).HasColumnName("BatchID");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.IsRetrieved).HasColumnName("isRetrieved");

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeZone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.Ipbatches)
                    .HasForeignKey(d => d.BatchId)
                    .HasConstraintName("FK_IPBatch_Batches");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
