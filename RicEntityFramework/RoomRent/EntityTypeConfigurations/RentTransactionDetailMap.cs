using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RentTransactionDetailMap : IEntityTypeConfiguration<RentTransactionDetail>
    {
        public void Configure(EntityTypeBuilder<RentTransactionDetail> builder)
        {
            builder.HasKey(o => o.Id);

            builder
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(t => t.RentTransaction)
                .WithMany(t => t.RentTransactionDetails)
                .HasForeignKey(t => t.TransactionId)
                .HasConstraintName("ForeignKey_RentTransactionDetails_RentTransaction_TransactionId");

            builder
                .HasOne(t => t.RentArrear)
                .WithMany(t => t.RentTransactionDetails)
                .HasForeignKey(t => t.RentArrearId)
                .HasConstraintName("ForeignKey_RentTransactionDetails_RentArrear_RentArrearId");

            builder.ToTable("RentTransactionDetails");
        }
    }
}
