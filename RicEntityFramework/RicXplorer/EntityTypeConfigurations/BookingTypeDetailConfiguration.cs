using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class BookingTypeDetailConfiguration : IEntityTypeConfiguration<BookingTypeDetail>
    {
        public void Configure(EntityTypeBuilder<BookingTypeDetail> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => t.BookingType)
                .WithMany(p => p.BookingTypeDetails)
                .HasForeignKey(f => f.BookingTypeId)
                .HasConstraintName("ForeignKey_BookingType_BookingTypeDetails_BookingTypeId");

            builder
                .HasOne(t => t.LookupTypeItem)
                .WithMany(p => p.BookingTypeDetails)
                .HasForeignKey(f => f.InclusionId)
                .HasConstraintName("ForeignKey_LookupTypeItem_BookingTypeDetails_InclusionId");

            builder.ToTable("BookingTypeDetails");
        }
    }
}
