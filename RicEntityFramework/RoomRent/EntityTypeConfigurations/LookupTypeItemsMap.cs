using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class LookupTypeItemsMap : IEntityTypeConfiguration<LookupTypeItems>
    {
        public void Configure(EntityTypeBuilder<LookupTypeItems> builder)
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

            builder.ToTable("LookupTypeItems");
        }
    }
}
