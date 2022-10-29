using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FundraiserAPI.EntityFramework
{
    public partial class FundraiserProjectContextRaw : DbContext
    {
        public FundraiserProjectContextRaw()
        {
        }

        public FundraiserProjectContextRaw(DbContextOptions<FundraiserProjectContextRaw> options)
            : base(options)
        {
        }

        public virtual DbSet<Fundraiser> Fundraisers { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<SessionToken> SessionTokens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=FundraiserDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fundraiser>(entity =>
            {
                entity.ToTable("Fundraiser");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.DonationTotal).HasColumnType("money");

                entity.Property(e => e.Goal).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.SecurityQuestion).HasMaxLength(50);

                entity.Property(e => e.SecurityQuestionAnswer).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<SessionToken>(entity =>
            {
                entity.ToTable("SessionToken");

                entity.Property(e => e.ExpiresOn).HasColumnType("datetime");

                entity.Property(e => e.SessionId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SessionTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__SessionTo__UserI__5DCAEF64");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
