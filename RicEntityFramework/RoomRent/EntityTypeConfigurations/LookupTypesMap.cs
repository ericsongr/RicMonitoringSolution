using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class LookupTypesMap : IEntityTypeConfiguration<LookupType>
    {
        public void Configure(EntityTypeBuilder<LookupType> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Name)
                .HasMaxLength(50);

            builder.ToTable("LookupTypes");
        }
    }
}
