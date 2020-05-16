using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class SettingMap : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("Settings");
        }
    }
}
