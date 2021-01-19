using System;
using System.Reflection;
using Audit.Core;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using RicModel.RicXplorer;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework
{
    public class RicDbContext : AuditDbContext
    {
        public RicDbContext(DbContextOptions<RicDbContext> options) :
            base(options)
        { }


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
        public DbSet<RentTransactionPayment> RentTransactionPayments { get; set; }

        public DbSet<AuditRenter> AuditRenters { get; set; }
        public DbSet<AuditRoom> AuditRooms { get; set; }
        public DbSet<AuditRentTransaction> AuditRentTransactions { get; set; }
        public DbSet<AuditRentTransactionPayment> AuditRentTransactionPayments { get; set; }

        public DbSet<MobileAppLog> MobileAppLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //register all fluent api entity type configuration or inherited by IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }

        public override void OnScopeCreated(AuditScope auditScope)
        {
            Database.BeginTransaction();
            //base.OnScopeCreated(auditScope);
        }

        public override void OnScopeSaving(AuditScope auditScope)
        {
            try
            {

                Database.CurrentTransaction.Commit(); ;
            }
            catch (Exception e)
            {
                Database.RollbackTransaction();
                
                Console.WriteLine(e);
            }
            //base.OnScopeSaving(auditScope);
        }
    }
}
