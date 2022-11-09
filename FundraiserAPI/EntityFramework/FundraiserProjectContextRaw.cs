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

        public virtual DbSet<Donation> Donations { get; set; } = null!;
        public virtual DbSet<Fundraiser> Fundraisers { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
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
            modelBuilder.Entity<Donation>(entity =>
            {
                entity.ToTable("Donation");

                entity.Property(e => e.AddressCity).HasMaxLength(50);

                entity.Property(e => e.AddressCountry).HasMaxLength(50);

                entity.Property(e => e.AddressState).HasMaxLength(50);

                entity.Property(e => e.AddressStreet1).HasMaxLength(50);

                entity.Property(e => e.AddressStreet2).HasMaxLength(50);

                entity.Property(e => e.AddressZip).HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.BankAccountNumber).HasMaxLength(50);

                entity.Property(e => e.CreditCardNumber).HasMaxLength(50);

                entity.Property(e => e.Cvv)
                    .HasMaxLength(3)
                    .HasColumnName("CVV");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PaymentType).HasMaxLength(50);
            });

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

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.BillingName).HasMaxLength(50);

                entity.Property(e => e.CardType).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.State).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(50);

                entity.Property(e => e.Zipcode).HasMaxLength(50);
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
