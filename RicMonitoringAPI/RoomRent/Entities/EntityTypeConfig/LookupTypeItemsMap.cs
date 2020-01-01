using Microsoft.EntityFrameworkCore;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class LookupTypeItemsMap
    {
        public static void AddMap(ModelBuilder modelBuilder) {

            modelBuilder.Entity<LookupTypeItems>().HasKey(t => t.Id);

            modelBuilder.Entity<LookupTypeItems>()
                .Property(t => t.Description)
                .HasMaxLength(100);

            modelBuilder.Entity<LookupTypeItems>()
                .HasOne(t => t.LookupTypes)
                .WithMany(p => p.LookupTypeItems)
                .HasForeignKey(f => f.LookupTypeId)
                .HasConstraintName("ForeignKey_LookupTypeItems_LookupTypes");

            modelBuilder.Entity<LookupTypeItems>().ToTable("LookupTypeItems");
        }
    }
}
