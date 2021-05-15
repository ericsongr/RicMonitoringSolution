using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(t => t.Account)
                .WithMany(p => p.Rooms)
                .HasForeignKey(f => f.AccountId)
                .HasConstraintName("FK_Rooms_Accounts_Id");

            builder.ToTable("Rooms");
        }
    }
}
