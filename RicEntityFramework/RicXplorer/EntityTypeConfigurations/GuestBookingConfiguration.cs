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
                .HasMaxLength(50);

            builder
                .Property(t => t.LastName)
                .HasMaxLength(50);

            builder
                .Property(t => t.Gender)
                .HasMaxLength(10);

            builder
                .Property(t => t.Birthday)
                .HasColumnType("date");

            builder
                .HasOne(t => t.LookupTypeItem)
                .WithMany(p => p.GuestBookings)
                .HasForeignKey(f => f.Ages)
                .HasConstraintName("ForeignKey_LookupTypeItems_GuestBookings");

            builder.ToTable("GuestBookings");
        }
    }
}
