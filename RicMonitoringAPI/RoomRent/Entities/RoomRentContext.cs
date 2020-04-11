using Microsoft.EntityFrameworkCore;
using RicMonitoringAPI.Common;
using RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig;
using RicMonitoringAPI.RicXplorer.Entities;
using RicMonitoringAPI.RicXplorer.Entities.EntityTypeConfig;

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
            SettingMap.AddMap(modelBuilder);

            RoomMap.AddMap(modelBuilder);
            RenterMap.AddMap(modelBuilder);

            BookedDetailMap.AddMap(modelBuilder);
            BookedPersonMap.AddMap(modelBuilder);

            LookupTypesMap.AddMap(modelBuilder);
            LookupTypeItemsMap.AddMap(modelBuilder);

            RentArrearMap.AddMap(modelBuilder);

            RentTransactionMap.AddMap(modelBuilder);
            RentTransactionDetailMap.AddMap(modelBuilder);

            //this.Database.ExecuteSqlCommand("RentTransactionBatchFile");


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
