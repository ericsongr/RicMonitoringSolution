using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class DbErrorLogMap : IEntityTypeConfiguration<DbErrorLog>
    {
        public void Configure(EntityTypeBuilder<DbErrorLog> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("DbErrorLogs");
        }
    }
}
