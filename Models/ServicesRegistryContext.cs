using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace SSORequestApplication.Models
{
    public partial class ServicesRegistryContext : DbContext
    {
        private readonly string connectionString;
        public ServicesRegistryContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }

        public ServicesRegistryContext(DbContextOptions<ServicesRegistryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AttributeRelease> AttributeRelease { get; set; }
        public virtual DbSet<Guidindex> Guidindex { get; set; }
        public virtual DbSet<Ssorequest> Ssorequest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttributeRelease>(entity =>
            {
                entity.HasKey(e => e.Pk);

                entity.Property(e => e.Pk).HasColumnName("PK");

                entity.Property(e => e.Attribute).HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Release).HasMaxLength(50);
            });

            modelBuilder.Entity<Guidindex>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.ToTable("GUIDIndex");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(50);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Ssorequest>(entity =>
            {
                entity.ToTable("SSORequest");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__SSOReque__3214EC2695E38E56")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminCompany).HasMaxLength(50);

                entity.Property(e => e.AdminEmail).HasMaxLength(50);

                entity.Property(e => e.AdminName).HasMaxLength(50);

                entity.Property(e => e.AdminPhone).HasMaxLength(50);

                entity.Property(e => e.AppName).HasMaxLength(50);

                entity.Property(e => e.LaunchDate).HasMaxLength(15);

                entity.Property(e => e.Memorandum).HasMaxLength(10);

                entity.Property(e => e.MetadataXmldev).HasColumnName("MetadataXMLDev");

                entity.Property(e => e.MetadataXmlprod).HasColumnName("MetadataXMLProd");

                entity.Property(e => e.Protocol).HasMaxLength(10);

                entity.Property(e => e.RequesterDepartment).HasMaxLength(50);

                entity.Property(e => e.RequesterEmail).HasMaxLength(50);

                entity.Property(e => e.RequesterName).HasMaxLength(50);

                entity.Property(e => e.RequesterPhone).HasMaxLength(50);

                entity.Property(e => e.RestrictedData).HasMaxLength(10);

                entity.Property(e => e.Reviewed).HasMaxLength(10);

                entity.Property(e => e.Samlinfo).HasColumnName("SAMLInfo");

                entity.Property(e => e.SubmittedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.TechnicalCompany).HasMaxLength(50);

                entity.Property(e => e.TechnicalEmail).HasMaxLength(50);

                entity.Property(e => e.TechnicalName).HasMaxLength(50);

                entity.Property(e => e.TechnicalPhone).HasMaxLength(50);

                entity.Property(e => e.UniqueId).HasMaxLength(50);

                entity.Property(e => e.UniqueIdAttr).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
