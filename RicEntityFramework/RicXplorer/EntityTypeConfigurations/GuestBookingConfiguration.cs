using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class GuestBookingConfiguration : IEntityTypeConfiguration<GuestBooking>
    {
        public void Configure(EntityTypeBuilder<GuestBooking> builder)
        {
            builder.HasKey(t => t.Id).HasName("PK_GuestBookings");

            builder
                .Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(t => t.Gender)
                .IsRequired()
                .HasMaxLength(10);

            builder
                .Property(t => t.Birthday)
                .IsRequired()
                .HasColumnType("date");

            builder
                .Property(t => t.Age)
                .IsRequired();

            builder
                .HasOne(t => t.LookupTypeItem)
                .WithMany(p => p.GuestBookings)
                .HasForeignKey(f => f.Ages)
                .HasConstraintName("ForeignKey_LookupTypeItems_GuestBookings")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.GuestBookingDetail)
                .WithMany(p => p.GuestBookings)
                .HasForeignKey(f => f.GuestBookingDetailId)
                .HasConstraintName("FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("GuestBookings");
        }
    }
}
