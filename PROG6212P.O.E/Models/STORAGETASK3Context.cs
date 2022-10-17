using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PROG6212P.O.E.ViewModels;

namespace PROG6212P.O.E.Models
{
    public partial class STORAGETASK3Context : DbContext
    {
        public STORAGETASK3Context()
        {
        }

        public STORAGETASK3Context(DbContextOptions<STORAGETASK3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Base> Base { get; set; }
        public virtual DbSet<Buy> Buy { get; set; }
        public virtual DbSet<Rent> Rent { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=JASON;Initial Catalog=STORAGETASK3;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Base>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PKey");

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Groceries).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Income).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Other).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Phone).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Tax).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Travel).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.WaterLights).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.Base)
                    .HasForeignKey<Base>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKey");
            });

            modelBuilder.Entity<Buy>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PKey3");

                entity.ToTable("buy");

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Deposit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PropPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.Buy)
                    .HasForeignKey<Buy>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKey3");
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PKey1");

                entity.ToTable("rent");

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ReantAmnt).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.Rent)
                    .HasForeignKey<Rent>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKey1");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__tblUser__C9F28457F0BAB538");

                entity.ToTable("tblUser");

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PKey2");

                entity.ToTable("vehicle");

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Deposit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Insurance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Model)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PurPrice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.Vehicle)
                    .HasForeignKey<Vehicle>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKey2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<PROG6212P.O.E.ViewModels.BudgBuyViewModel> BudgBuyViewModel { get; set; }

        public DbSet<PROG6212P.O.E.ViewModels.Tables> Tables { get; set; }
    }
}
