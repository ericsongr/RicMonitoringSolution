using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RentTranasactionMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RentTransaction>().HasKey(t => t.Id);

            modelBuilder.Entity<RentTransaction>()
                .HasOne(t => t.Room)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("ForeignKey_RentTransaction_Room");

            modelBuilder.Entity<RentTransaction>()
                .HasOne(t => t.Renter)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("ForeignKey_RentTransaction_Renter");

            modelBuilder.Entity<RentTransaction>().ToTable("RentTransaction");

        }
    }
}
