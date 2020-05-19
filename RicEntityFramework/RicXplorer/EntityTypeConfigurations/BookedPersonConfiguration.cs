using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class BookedPersonConfiguration : IEntityTypeConfiguration<BookedPerson>
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
