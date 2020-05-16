using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RicMonitoringAPI.RoomRent.Entities.EntityTypeConfig
{
    public class RoomMap : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            builder.ToTable("Rooms");
        }
    }
}
