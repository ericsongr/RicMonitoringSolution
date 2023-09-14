using System;
using System.Reflection;
using Audit.Core;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using RicModel.CostMonitoring;
using RicModel.RicXplorer;
using RicModel.RoomRent;
using RicModel.RoomRent.Audits;
using RicModel.ToolsInventory;

namespace RicEntityFramework
{
    public class RicDbContext : AuditDbContext
    {
        public RicDbContext(DbContextOptions<RicDbContext> options) :
            base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<RentTransaction> RentTransactions { get; set; }
        public DbSet<RentTransactionDetail> RentTransactionDetails { get; set; }


        //maintenance
        public DbSet<LookupType> LookupTypes { get; set; }
        public DbSet<LookupTypeItem> LookupTypeItems { get; set; }

        public DbSet<MonthlyRentBatch> MonthlyRentBatch { get; set; }

        public DbSet<RentArrear> RentArrears { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<RentTransactionPayment> RentTransactionPayments { get; set; }

        public DbSet<MobileAppLog> MobileAppLogs { get; set; }

        public DbSet<SmsGateway> SmsGateway { get; set; }

        public DbSet<RenterCommunicationHistory> RenterCommunicationHistory { get; set; }
        
        public DbSet<AccountBillingItem> AccountBillingItems { get; set; }
        
        //Audit Tables
        public DbSet<AuditAccount> AuditAccounts { get; set; }
        public DbSet<AuditRenter> AuditRenters { get; set; }
        public DbSet<AuditRoom> AuditRooms { get; set; }
        public DbSet<AuditRentTransaction> AuditRentTransactions { get; set; }
        public DbSet<AuditRentTransactionPayment> AuditRentTransactionPayments { get; set; }

        //ricxplorer - start
        
        public DbSet<GuestBooking> GuestBookings { get; set; }
        public DbSet<GuestBookingDetail> GuestBookingDetails { get; set; }
        public DbSet<GuestBookingDate> GuestBookingDates { get; set; }
        public DbSet<BookingType> BookingTypes { get; set; }
        public DbSet<BookingTypeInclusion> BookingTypeInclusions { get; set; }
        public DbSet<BookingTypeImage> BookingTypeImages { get; set; }
        public DbSet<AccountProduct> AccountProducts { get; set; }
        public DbSet<AccountProductCategory> AccountProductCategories { get; set; }

        //ricxplorer - end

        //cost monitoring - start

        public DbSet<CostItem> CostItems { get; set; }

        //cost monitoring - end

        // tool inventory - start
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolInventory> ToolsInventory { get; set; }
        // tool inventory - end

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
