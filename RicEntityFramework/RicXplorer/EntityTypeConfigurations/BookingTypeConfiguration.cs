using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class BookingTypeConfiguration : IEntityTypeConfiguration<BookingType>
    {
        public void Configure(EntityTypeBuilder<BookingType> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Name)
                .HasMaxLength(100);

            builder
                .Property(t => t.Image)
                .HasMaxLength(100);

            builder.ToTable("BookingTypes");
        }
    }
}
