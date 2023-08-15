using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.CostMonitoring;
using RicModel.RoomRent;

namespace RicEntityFramework.CostMonitoring.EntityTypeConfigurations
{
    public class TransactionCostConfiguration : IEntityTypeConfiguration<TransactionCost>
    {
        public void Configure(EntityTypeBuilder<TransactionCost> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(o => o.CostItem)
                .WithMany(o => o.TransactionCosts)
                .HasForeignKey(o => o.CostItemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ForeignKey_TransactionCost_CostItems");

            builder.HasOne(o => o.CostCategory)
                .WithMany(o => o.TransactionCosts)
                .HasForeignKey(o => o.CostCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ForeignKey_TransactionCost_LookupTypeItems");

            builder.HasQueryFilter(o => !o.IsDeleted);

            builder.ToTable("TransactionCost");
        }
    }
}
