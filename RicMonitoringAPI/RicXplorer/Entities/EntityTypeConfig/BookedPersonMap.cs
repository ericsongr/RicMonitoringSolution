using Microsoft.EntityFrameworkCore;
using RicMonitoringAPI.RoomRent.Entities;

namespace RicMonitoringAPI.RicXplorer.Entities.EntityTypeConfig
{
    public class BookedPersonMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<BookedPerson>().HasKey(t => t.Id);

            modelBuilder.Entity<BookedPerson>()
                .Property(t => t.FirstName)
                .HasMaxLength(50);

            modelBuilder.Entity<BookedPerson>()
                .Property(t => t.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<BookedPerson>()
                .HasOne(t => t.LookupTypeItems)
                .WithMany(p => p.BookedPersons)
                .HasForeignKey(f => f.Type)
                .HasConstraintName("ForeignKey_LookupTypeItems_BookedPersons");

            modelBuilder.Entity<LookupType>().ToTable("BookedPersons");
        }
    }
}
