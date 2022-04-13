using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class BookingTypeImageConfiguration : IEntityTypeConfiguration<BookingTypeImage>
    {
        public void Configure(EntityTypeBuilder<BookingTypeImage> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.ImageName)
                .HasMaxLength(100);

            builder
                .HasOne(t => t.BookingType)
                .WithMany(p => p.BookingTypeImages)
                .HasForeignKey(f => f.BookingTypeId)
                .HasConstraintName("ForeignKey_BookingType_BookingTypeImages_BookingTypeId");

            builder.ToTable("BookingTypeImages");
        }
    }
}
