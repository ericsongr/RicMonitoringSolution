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
                .IsRequired()
                .HasColumnName("ArrivalDateLocal");

            builder
                .Property(t => t.DepartureDate)
                .IsRequired()
                .HasColumnName("DepartureDateLocal");

            builder
                .Property(t => t.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(t => t.LanguagesSpoken)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(t => t.Contact)
                .IsRequired()
                .HasMaxLength(15);

            builder
                .Property(t => t.ContactPerson)
                .HasMaxLength(100); //optional

            builder
                .Property(t => t.LeaveMessage) //optional
                .HasMaxLength(1000);

            builder.ToTable("GuestBookingDetails");
        }
    }
}
