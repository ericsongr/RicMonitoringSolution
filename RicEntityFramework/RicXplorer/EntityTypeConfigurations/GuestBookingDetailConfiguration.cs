using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class GuestBookingDetailConfiguration : IEntityTypeConfiguration<GuestBookingDetail>
    {
        public void Configure(EntityTypeBuilder<GuestBookingDetail> builder)
        {
            builder.HasKey(t => t.Id).HasName("PK_GuestBookingDetails");

            builder
                .Property(t => t.ArrivalDate)
                .HasColumnName("ArrivalDateLocal");

            builder
                .Property(t => t.DepartureDate)
                .HasColumnName("DepartureDateLocal");

            builder
                .Property(t => t.Country)
                .HasMaxLength(100);

            builder
                .Property(t => t.LanguagesSpoken)
                .HasMaxLength(100);

            builder
                .Property(t => t.Email)
                .HasMaxLength(50);

            builder
                .Property(t => t.Contact)
                .HasMaxLength(15);

            builder
                .Property(t => t.ContactPerson)
                .HasMaxLength(100);

            builder
                .Property(t => t.LeaveMessage)
                .HasMaxLength(1000);

            builder.ToTable("GuestBookingDetails");
        }
    }
}
