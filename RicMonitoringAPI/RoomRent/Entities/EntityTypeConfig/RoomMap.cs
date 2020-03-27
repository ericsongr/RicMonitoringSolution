using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RoomMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Room>().HasKey(t => t.Id);

            modelBuilder.Entity<Room>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RentArrear>().ToTable("Rooms");

        }
    }
}
