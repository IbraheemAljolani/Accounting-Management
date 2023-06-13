using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace AccountingManagement.Core.Models
{
    public partial class AccountingManagementContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private static string AMConnection;
        public AccountingManagementContext()
        {
        }

        public AccountingManagementContext(DbContextOptions<AccountingManagementContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            AMConnection = _configuration.GetConnectionString("EFConnection");
        }

        public virtual DbSet<AccountTable> AccountTables { get; set; } = null!;
        public virtual DbSet<LoginTable> LoginTables { get; set; } = null!;
        public virtual DbSet<TransactionTable> TransactionTables { get; set; } = null!;
        public virtual DbSet<UserTable> UserTables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AMConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTable>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Account___B19E45C9AF189540");

                entity.ToTable("Account_Table");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("Account_Number");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.Currency)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTime_UTC");

                entity.Property(e => e.ServerDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Server_DateTime");

                entity.Property(e => e.UpdateDateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_DateTime_UTC");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccountTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Account_T__User___398D8EEE");
            });

            modelBuilder.Entity<LoginTable>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK__Login_Ta__D788686737710919");

                entity.ToTable("Login_Table");

                entity.Property(e => e.LoginId).HasColumnName("Login_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.LastLogout).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LoginTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Login_Tab__User___403A8C7D");
            });

            modelBuilder.Entity<TransactionTable>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__Transact__9A8D56254FF1EC42");

                entity.ToTable("Transaction_Table");

                entity.Property(e => e.TransactionId).HasColumnName("Transaction_ID");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.DateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTime_UTC");

                entity.Property(e => e.ServerDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Server_DateTime");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_DateTime_UTC");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TransactionTables)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Transacti__Accou__3D5E1FD2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TransactionTables)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__User___3C69FB99");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_Tab__206D91900DA3BFF1");

                entity.ToTable("User_Table");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Of_Birth");

                entity.Property(e => e.DateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("DateTime_UTC");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.ServerDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Server_DateTime");

                entity.Property(e => e.UpdateDateTimeUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("Update_DateTime_UTC");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
