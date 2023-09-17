using Microsoft.EntityFrameworkCore;
using PaymentAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Infrastructure.Data
{
    public class PaymentdbContext : DbContext
    {
        public PaymentdbContext()
        {
        }
        public static string ConnectionString { get; set; }
        public virtual DbSet<tbl_AccountStatement> tbl_AccountStatements { get; set; }
        public virtual DbSet<tbl_NIPTransaction> tbl_PaymentOutwardTransactions { get; set; }
        public virtual DbSet<tbl_PaymentTransaction> tbl_PaymentInwardTransactions { get; set; }
        public virtual DbSet<tbl_PaymentProfile> tbl_PaymentProfiles { get; set; }
        public virtual DbSet<tbl_Activity_log> tbl_Activity_logs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_AccountStatement>(entity =>
            {
                entity.Property(e => e.Id)
                                    .HasColumnName("id")
                                    .HasDefaultValueSql("nextval('account.tbl_AccountStatement_seq'::regclass)");
               
            });
            modelBuilder.HasSequence("tbl_AccountStatement_seq", "account");
          

            modelBuilder.Entity<tbl_NIPTransaction>(entity =>
            {
                entity.Property(e => e.Id)
                                    .HasColumnName("id")
                                    .HasDefaultValueSql("nextval('account.tbl_PaymentOutwardTransaction_seq'::regclass)");
                entity.Property(e => e.BankCode).IsRequired();
                entity.Property(e => e.BankName)
                                    .IsRequired();
            });
            modelBuilder.HasSequence("tbl_PaymentOutwardTransaction_seq", "account");

            modelBuilder.Entity<tbl_AccountStatement>(entity =>
            {


                entity.HasOne(d => d.tblPaymentProfile)
                    .WithMany(p => p.tblAccountStatement)
                    .HasForeignKey(d => d.PaymentProfileId);
            });

            //modelBuilder.Entity<tbl_NIPTransaction>(entity =>
            //{


            //    entity.HasOne(d => d.tblAccountStatement)
            //        .WithMany(p => p.tblPaymentOutwardTransaction)
            //        .HasForeignKey(d => d.AccountStatementId);
            //});

            modelBuilder.Entity<tbl_PaymentTransaction>(entity =>
            {
                entity.Property(e => e.Id)
                                    .HasColumnName("id")
                                    .HasDefaultValueSql("nextval('account.tbl_PaymentInwardTransaction_seq'::regclass)");
               
            });
            modelBuilder.HasSequence("tbl_PaymentInwardTransaction_seq", "account");
           
            modelBuilder.Entity<tbl_PaymentTransaction>(entity =>
            {


                entity.HasOne(d => d.tblPaymentProfile)
                    .WithMany(p => p.tblPaymentInwardTransaction)
                    .HasForeignKey(d => d.PaymentProfileId);
            });
            modelBuilder.Entity<tbl_NIPTransaction>(entity =>
            {


                entity.HasOne(d => d.tblPaymentInwardTransaction)
                    .WithMany(p => p.tbl_NIPTransaction)
                    .HasForeignKey(d => d.PaymentTransactionId);
            });

            modelBuilder.Entity<tbl_PaymentProfile>(entity =>
            {
                entity.Property(e => e.Id)
                                    .HasColumnName("id")
                                    .HasDefaultValueSql("nextval('account.tbl_PaymentProfile_seq'::regclass)");
                entity.Property(e => e.BusinessName).IsRequired();
                entity.Property(e => e.ContactName).IsRequired();
                entity.Property(e => e.ContactSurname)
                                    .IsRequired();
                entity.Property(e => e.MerchantNumber)
                                   .IsRequired();
                entity.Property(e => e.NationalIDNumber)
                                  .IsRequired();
                entity.Property(e => e.Name)
                                .IsRequired();
                entity.Property(e => e.Surname)
                                .IsRequired();
            });
            modelBuilder.HasSequence("tbl_PaymentProfile_seq", "account");
          
        }
    }
}
