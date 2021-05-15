using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class RenterConfiguration : IEntityTypeConfiguration<Renter>
    {
        public void Configure(EntityTypeBuilder<Renter> builder)
        {
            builder.HasKey(t => t.Id);

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

            builder
                .HasOne(t => t.Room)
                .WithMany(p => p.Renters)
                .HasForeignKey(f => f.RoomId)
                .HasConstraintName("ForeignKey_Renter_Room");

            builder
                .HasOne(t => t.Account)
                .WithMany(p => p.Renters)
                .HasForeignKey(f => f.AccountId)
                .HasConstraintName("FK_Renters_Accounts_Id");

            builder.ToTable("Renters");
        }
    }
}
