using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class LookupTypeItemsConfiguration : IEntityTypeConfiguration<LookupTypeItem>
    {
        public void Configure(EntityTypeBuilder<LookupTypeItem> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Description)
                .HasMaxLength(100);

            builder
                .HasOne(t => t.LookupTypes)
                .WithMany(p => p.LookupTypeItems)
                .HasForeignKey(f => f.LookupTypeId)
                .HasConstraintName("ForeignKey_LookupTypeItems_LookupTypes");

            builder.ToTable("LookupTypeItem");
        }
    }
}
