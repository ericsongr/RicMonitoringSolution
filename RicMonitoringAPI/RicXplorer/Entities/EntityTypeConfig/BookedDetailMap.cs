using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RicXplorer.Entities.EntityTypeConfig
{
    public class BookedDetailMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<BookedDetail>().HasKey(t => t.Id);

            modelBuilder.Entity<BookedDetail>()
                .Property(t => t.Country)
                .HasMaxLength(100);

            modelBuilder.Entity<BookedDetail>()
                .Property(t => t.LanguagesSpoken)
                .HasMaxLength(100);

            modelBuilder.Entity<BookedDetail>()
               .Property(t => t.Email)
               .HasMaxLength(50);

            modelBuilder.Entity<BookedDetail>()
               .Property(t => t.Contact)
               .HasMaxLength(15);

            modelBuilder.Entity<BookedDetail>()
               .Property(t => t.LeaveMessage)
               .HasMaxLength(1000);

            modelBuilder.Entity<BookedDetail>().ToTable("BookedDetails");
        }
    }
}
