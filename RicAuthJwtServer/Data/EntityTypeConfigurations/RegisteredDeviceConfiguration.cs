
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Data.EntityTypeConfigurations
{
    public class RegisteredDeviceConfiguration : IEntityTypeConfiguration<RegisteredDevice>
    {
        public void Configure(EntityTypeBuilder<RegisteredDevice> builder)
        {
            builder.HasKey(o => o.Id)
                .HasName("PK_RegisteredDevices");

            builder.HasOne(o => o.User)
                .WithMany(o => o.RegisteredDevices)
                .HasForeignKey(o => o.AspNetUsersId)
                .HasConstraintName("FK_RegisteredDevices_AspNetUsers")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("RegisteredDevices");
        }
    }
}
