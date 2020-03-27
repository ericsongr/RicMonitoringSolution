using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RentTransactionDetailMap
    {
        public static void AddMap(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentTransactionDetail>().HasKey(o => o.Id);

            modelBuilder.Entity<RentTransactionDetail>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentTransactionDetail>()
                .HasOne(t => t.RentTransaction)
                .WithMany(t => t.RentTransactionDetails)
                .HasForeignKey(t => t.TransactionId)
                .HasConstraintName("ForeignKey_RentTransactionDetails_RentTransaction_TransactionId");

            modelBuilder.Entity<RentTransactionDetail>()
                .HasOne(t => t.RentArrear)
                .WithMany(t => t.RentTransactionDetails)
                .HasForeignKey(t => t.RentArrearId)
                .HasConstraintName("ForeignKey_RentTransactionDetails_RentArrear_RentArrearId");

            modelBuilder.Entity<RentTransactionDetail>().ToTable("RentTransactionDetails");
        }
    }
}
