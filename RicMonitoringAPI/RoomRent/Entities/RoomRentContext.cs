using Microsoft.EntityFrameworkCore;
using RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig;

namespace RicMonitoringAPI.RoomRent.Entities
{
    public class RoomRentContext : DbContext
    {
        public RoomRentContext(DbContextOptions<RoomRentContext> options) : 
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RenterMap.AddMap(modelBuilder);
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<RentTransaction> RentTransactions { get; set; }
    }
}
