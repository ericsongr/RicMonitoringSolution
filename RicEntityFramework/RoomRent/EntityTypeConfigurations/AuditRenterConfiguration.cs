
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class AuditRenterConfiguration : IEntityTypeConfiguration<AuditRenter>
    {
        public void Configure(EntityTypeBuilder<AuditRenter> builder)
        {
            builder.HasKey(o => o.AuditRenterId)
                .HasName("PK_AuditRenters");

            builder.Property(t => t.Id);

            builder
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(t => t.AdvanceMonths)
                .IsRequired();

            builder
                .Property(t => t.AdvancePaidDate)
                .IsRequired();

            builder
                .Property(t => t.StartDate)
                .IsRequired();

            builder
                .Property(t => t.DueDay)
                .IsRequired();

            builder
                .Property(t => t.NoOfPersons)
                .IsRequired();

            builder
                .Property(t => t.BalanceAmount)
                .HasColumnType("decimal(18,2)");

            builder
                .Property(t => t.TotalPaidAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.Renter)
                .WithMany(o => o.AuditRenters)
                .HasForeignKey(o => o.Id)
                .HasConstraintName("ForeignKey_AuditRenters_Renters")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Room)
                .WithMany(o => o.AuditRenters)
                .HasForeignKey(o => o.RoomId)
                .HasConstraintName("ForeignKey_AuditRenters_Room")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("AuditRenters");
        }
    }
}
