using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RentArrearMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<RentArrear>().HasKey(t => t.Id);

            modelBuilder.Entity<RentArrear>()
                .Property(t => t.UnpaidAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentArrear>()
                .HasOne(t => t.RentTransaction)
                .WithMany(p => p.RentArrears)
                .HasForeignKey(f => f.RentTransactionId)
                .HasConstraintName("ForeignKey_RentArrears_RentTransaction_RentTransactionId");

            modelBuilder.Entity<RentArrear>()
                .HasOne(t => t.Renter)
                .WithMany(p => p.RentArrears)
                .HasForeignKey(f => f.RenterId)
                .HasConstraintName("ForeignKey_RentArrears_Renter_RenterId");

            modelBuilder.Entity<RentArrear>().ToTable("RentArrears");

        }
    }
}
