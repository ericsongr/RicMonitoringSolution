using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RenterMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Renter>().HasKey(t => t.Id);

            modelBuilder.Entity<Renter>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Renter>()
                .Property(t => t.AdvanceMonths)
                .IsRequired();

            modelBuilder.Entity<Renter>()
                .Property(t => t.AdvancePaidDate)
                .IsRequired();

            modelBuilder.Entity<Renter>()
                .Property(t => t.StartDate)
                .IsRequired();

            modelBuilder.Entity<Renter>()
                .Property(t => t.DueDay)
                .IsRequired();

            modelBuilder.Entity<Renter>()
                .Property(t => t.NoOfPersons)
                .IsRequired();

            modelBuilder.Entity<Renter>()
                .HasOne(t => t.Room)
                .WithMany(p => p.Renters)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("ForeignKey_Renter_Room");

            modelBuilder.Entity<Renter>()
                .HasOne(t => t.RentTransaction)
                .WithMany(p => p.Renters)
                .HasForeignKey(f => f.RentTransactionId)
                .HasConstraintName("ForeignKey_Renter_RentTransaction");

            modelBuilder.Entity<Renter>().ToTable("Renters");

        }
    }
}
