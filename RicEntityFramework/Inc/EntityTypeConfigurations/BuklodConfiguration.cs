using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RicEntityFramework.Inc.EntityTypeConfigurations
{
    public class BuklodConfiguration : IEntityTypeConfiguration<RicModel.Inc.IncBuklod>
    {
        public void Configure(EntityTypeBuilder<RicModel.Inc.IncBuklod> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasQueryFilter(o => !o.IsDeleted);

            builder.ToTable("IncBuklod");
        }
    }
}
