using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class AuditRentTransactionPaymentConfiguration : IEntityTypeConfiguration<AuditRentTransactionPayment>
    {
        public void Configure(EntityTypeBuilder<AuditRentTransactionPayment> builder)
        {
            builder.HasKey(o => o.Id).HasName("PK_AuditRentTransactionPayments");

            builder.Property(o => o.Amount).HasColumnType("decimal(18,2)");

            builder.HasIndex(o => o.DatePaid).HasName("IDX_AuditRentTransactionPayments_DatePaid");

            builder.HasOne(o => o.RentTransactionPayment)
                .WithMany(o => o.AuditRentTransactionPayments)
                .HasForeignKey(o => o.Id)
                .HasConstraintName("FK_AuditRentTransactionPayments_RentTransactions_AuditRentTransactionPaymentId7")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
