using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RentArrearConfiguration : IEntityTypeConfiguration<RentArrear>
    {
        public void Configure(EntityTypeBuilder<RentArrear> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.UnpaidAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.ManualEntryDateTime)
                .HasColumnName("ManualEntryDateTimeLocal");

            builder.Property(t => t.ProcessedDateTime)
                .HasColumnName("ProcessedDateTimeUtc");

            builder
                .HasOne(t => t.RentTransaction)
                .WithMany(p => p.RentArrears)
                .HasForeignKey(f => f.RentTransactionId)
                .HasConstraintName("ForeignKey_RentArrears_RentTransaction_RentTransactionId");

            builder
                .HasOne(t => t.Renter)
                .WithMany(p => p.RentArrears)
                .HasForeignKey(f => f.RenterId)
                .HasConstraintName("ForeignKey_RentArrears_Renter_RenterId");

            builder.ToTable("RentArrears");
        }
    }
}
