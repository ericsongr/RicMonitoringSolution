using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.ToolsInventory;

namespace RicEntityFramework.ToolsInventory.EntityTypeConfigurations
{
    public class ToolInventoryConfiguration : IEntityTypeConfiguration<ToolInventory>
    {
        public void Configure(EntityTypeBuilder<ToolInventory> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(o => o.Tool)
                .WithMany(o => o.ToolsInventory)
                .HasForeignKey(o => o.ToolId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ToolsInventory_ToolsInventory_Tools");

            builder.HasOne(o => o.LookupTypeItemAction)
                .WithMany(o => o.ToolInventoryActions)
                .HasForeignKey(o => o.Action)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ToolsInventory_LookupTypeItem_Action");

            builder.HasOne(o => o.LookupTypeItemStatus)
                .WithMany(o => o.ToolInventoryStatuses)
                .HasForeignKey(o => o.Status)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ToolsInventory_LookupTypeItem_Status");

            builder.HasQueryFilter(o => !o.IsDeleted);

            builder.ToTable("ToolsInventory");
        }
    }
}
