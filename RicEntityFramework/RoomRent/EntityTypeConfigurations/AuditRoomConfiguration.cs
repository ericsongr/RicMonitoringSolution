using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class AuditRoomConfiguration : IEntityTypeConfiguration<AuditRoom>
    {
        public void Configure(EntityTypeBuilder<AuditRoom> builder)
        {
            builder.HasKey(o => o.AuditRoomId)
                .HasName("PK_AuditRooms");

            builder.Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Room)
                .WithMany(o => o.AuditRooms)
                .HasForeignKey(o => o.Id)
                .HasConstraintName("FK_AuditRooms_Rooms_RoomId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("AuditRooms");
        }
    }
}
