using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RicModel.RicXplorer;
using RicModel.RoomRent;

namespace RicEntityFramework
{
    public class RicDbContext : DbContext
    {
        public RicDbContext(DbContextOptions<RicDbContext> options) :
            base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //register all fluent api entity type configuration or inherited by IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<RentTransaction> RentTransactions { get; set; }
        public DbSet<RentTransactionDetail> RentTransactionDetails { get; set; }

        public DbSet<BookedPerson> BookedPersons { get; set; }
        public DbSet<BookedDetail> BookedDetails { get; set; }

        //maintenance
        public DbSet<LookupType> LookupTypes { get; set; }
        public DbSet<LookupTypeItems> LookupTypeItems { get; set; }

        public DbSet<MonthlyRentBatch> MonthlyRentBatch { get; set; }

        public DbSet<RentArrear> RentArrears { get; set; }
        public DbSet<Setting> Settings { get; set; }

    }
}
