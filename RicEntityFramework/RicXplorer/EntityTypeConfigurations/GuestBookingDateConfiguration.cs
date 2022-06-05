using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class GuestBookingDateConfiguration : IEntityTypeConfiguration<GuestBookingDate>
    {
        public void Configure(EntityTypeBuilder<GuestBookingDate> builder)
        {
            builder.HasKey(t => t.Id).HasName("PK_GuestBookings");

            builder
                .HasOne(t => t.GuestBookingDetail)
                .WithMany(p => p.GuestBookingDates)
                .HasForeignKey(f => f.GuestBookingDetailId)
                .HasConstraintName("FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("GuestBookingDates");
        }
    }
}
