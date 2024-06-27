using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class CheckListForCheckInOutGuestConfiguration : IEntityTypeConfiguration<CheckListForCheckInOutGuest>
    {
        public void Configure(EntityTypeBuilder<CheckListForCheckInOutGuest> builder)
        {
            builder.HasKey(t => t.Id).HasName("PK_CheckListForCheckInOutGuests");

            builder
                .HasOne(t => t.LookupType)
                .WithMany(p => p.CheckListForCheckInOutGuests)
                .HasForeignKey(f => f.LookupId)
                .HasConstraintName("FK_LookupType_CheckListForCheckInOutGuest_LookupId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.LookupTypeItem)
                .WithMany(p => p.CheckListForCheckInOutGuests)
                .HasForeignKey(f => f.LookupTypeItemId)
                .HasConstraintName("FK_LookupTypeItem_CheckListForCheckInOutGuest_LookupTypeItemId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("CheckListForCheckInOutGuests");
        }
    }
}
