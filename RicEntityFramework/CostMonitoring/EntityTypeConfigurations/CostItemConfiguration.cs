using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.CostMonitoring;
using RicModel.RoomRent;

namespace RicEntityFramework.CostMonitoring.EntityTypeConfigurations
{
    public class CostItemConfiguration : IEntityTypeConfiguration<CostItem>
    {
        public void Configure(EntityTypeBuilder<CostItem> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasQueryFilter(o => !o.IsDeleted);

            builder.ToTable("CostItems");
        }
    }
}
