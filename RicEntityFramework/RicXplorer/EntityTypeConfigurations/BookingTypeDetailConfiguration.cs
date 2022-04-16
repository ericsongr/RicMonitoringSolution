using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class BookingTypeInclusionConfiguration : IEntityTypeConfiguration<BookingTypeInclusion>
    {
        public void Configure(EntityTypeBuilder<BookingTypeInclusion> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => t.BookingType)
                .WithMany(p => p.BookingTypeInclusions)
                .HasForeignKey(f => f.BookingTypeId)
                .HasConstraintName("ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId");

            builder
                .HasOne(t => t.LookupTypeItem)
                .WithMany(p => p.BookingTypeInclusions)
                .HasForeignKey(f => f.InclusionId)
                .HasConstraintName("ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId");

            builder.ToTable("BookingTypeInclusions");
        }
    }
}
