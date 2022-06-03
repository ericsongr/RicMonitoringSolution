using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class AccountProductCategoryConfiguration : IEntityTypeConfiguration<AccountProductCategory>
    {
        public void Configure(EntityTypeBuilder<AccountProductCategory> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Name)
                .HasMaxLength(100);

            builder.HasOne(o => o.Account)
                .WithMany(o => o.AccountProductCategories)
                .HasForeignKey(o => o.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AccountProductCategory_Accounts");

            builder.ToTable("AccountProductCategory");
        }
    }
}
