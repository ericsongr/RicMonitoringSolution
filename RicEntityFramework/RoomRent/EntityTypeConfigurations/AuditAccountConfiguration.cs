using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RoomRent.Audits;

namespace RicEntityFramework.RoomRent.EntityTypeConfigurations
{
    public class AuditAccountConfiguration : IEntityTypeConfiguration<AuditAccount>
    {
        public void Configure(EntityTypeBuilder<AuditAccount> builder)
        {
            builder.HasKey(t => t.AuditAccountId);

            builder
                .HasOne(t => t.Account)
                .WithMany(p => p.AuditAccounts)
                .HasForeignKey(f => f.Id)
                .HasConstraintName("FK_AuditAccounts_Accounts_Id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("AuditAccounts");
        }
    }
}
