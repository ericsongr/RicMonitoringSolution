using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RicMonitoringAPI.RicXplorer.Entities.EntityTypeConfig
{
    public class BookedPersonMap : IEntityTypeConfiguration<BookedPerson>
    {
        public void Configure(EntityTypeBuilder<BookedPerson> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.FirstName)
                .HasMaxLength(50);

            builder
                .Property(t => t.LastName)
                .HasMaxLength(50);

            builder
                .HasOne(t => t.LookupTypeItems)
                .WithMany(p => p.BookedPersons)
                .HasForeignKey(f => f.Ages)
                .HasConstraintName("ForeignKey_LookupTypeItems_BookedPersons");

            builder.ToTable("BookedPersons");
        }
    }
}
