using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class AuditRentTransactionConfiguration : IEntityTypeConfiguration<AuditRentTransaction>
    {

        public void Configure(EntityTypeBuilder<AuditRentTransaction> builder)
        {
            builder.HasKey(t => t.AuditRentTransactionId).HasName("PK_AuditRentTransactionPayments");

            builder
                .Property(p => p.PaidAmount)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(p => p.Balance)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ExcessPaidAmount)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(p => p.TotalAmountDue)
                .HasColumnType("decimal(18,2)");

            builder
                .HasOne(t => t.Room)
                .WithMany(p => p.AuditRentTransactions)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("FK_AuditRentTransactions_Room_RoomId");

            builder
                .HasOne(t => t.Renter)
                .WithMany(p => p.AuditRentTransactions)
                .HasForeignKey(f => f.RenterId)
                .HasConstraintName("FK_AuditRentTransactions_Renters_RenterId");

            builder
                .HasOne(t => t.RentTransaction)
                .WithMany(p => p.AuditRentTransactions)
                .HasForeignKey(f => f.Id)
                .HasConstraintName("FK_AuditRentTransactions_RentTransactions_Id");

            builder.ToTable("AuditRentTransactions");
        }
    }
}
