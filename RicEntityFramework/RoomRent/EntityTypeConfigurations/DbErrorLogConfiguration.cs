using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class DbErrorLogConfiguration : IEntityTypeConfiguration<DbErrorLog>
    {
        public void Configure(EntityTypeBuilder<DbErrorLog> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("DbErrorLogs");
        }

    }
}
