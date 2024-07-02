using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class GuestCheckListConfiguration : IEntityTypeConfiguration<GuestCheckList>
    {
        public void Configure(EntityTypeBuilder<GuestCheckList> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t => t.GuestBookingDetail)
                .WithMany(p => p.GuestCheckLists)
                .HasForeignKey(f => f.GuestBookingDetailId)
                .HasConstraintName("FK_GuestCheckLists_LookupTypeItems_GuestBookingDetailId");

            builder
                .HasOne(t => t.CheckList)
                .WithMany(p => p.GuestCheckLists)
                .HasForeignKey(f => f.CheckListId)
                .HasConstraintName("FK_GuestCheckLists_LookupTypeItems_CheckListId");

            builder.ToTable("GuestCheckLists");
        }
    }
}
