using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RentTransactionMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RentTransaction>().HasKey(t => t.Id);

            modelBuilder.Entity<RentTransaction>()
                .Property(p => p.PaidAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentTransaction>()
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentTransaction>()
                .Property(p => p.TotalAmountDue)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentTransaction>()
                .Property(p => p.AdjustmentBalancePaymentDueAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentTransaction>()
                .HasOne(t => t.Room)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("ForeignKey_RentTransaction_Room_RoomId");

            modelBuilder.Entity<RentTransaction>()
                .HasOne(t => t.Renter)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RenterId)
                .HasConstraintName("ForeignKey_RentTransaction_Renter_RenterId");
                
            modelBuilder.Entity<RentTransaction>().ToTable("RentTransactions");

        }
    }
}
