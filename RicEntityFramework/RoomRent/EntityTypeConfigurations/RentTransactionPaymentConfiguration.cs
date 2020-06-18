using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RentTransactionPaymentConfiguration : IEntityTypeConfiguration<RentTransactionPayment>
    {
        public void Configure(EntityTypeBuilder<RentTransactionPayment> builder)
        {
            builder.HasKey(o => o.Id).HasName("PK_RentTransactionPayments");

            builder.Property(o => o.Amount).HasColumnType("decimal(18,2)");

            builder.HasIndex(o => o.DatePaid).HasName("IDX_RentTransactionPayments_DatePaid");

            builder.HasOne(o => o.RentTransaction)
                .WithMany(o => o.RentTransactionPayments)
                .HasForeignKey(o => o.RentTransactionId)
                .HasConstraintName("FK_RentTransactionPayments_RentTransactions_RentTransactionId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
