using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RentTransactionConfiguration : IEntityTypeConfiguration<RentTransaction>
    {

        public void Configure(EntityTypeBuilder<RentTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(p => p.PaidAmount)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(p => p.TotalAmountDue)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(p => p.AdjustmentBalancePaymentDueAmount)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(t => t.Room)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("FK_RentTransactions_Room_RoomId");

            builder
                .HasOne(t => t.Renter)
                .WithMany(p => p.RentTransactions)
                .HasForeignKey(f => f.RenterId)
                .HasConstraintName("FK_RentTransactions_Renters_RenterId");

            builder.ToTable("RentTransactions");
        }
    }
}
