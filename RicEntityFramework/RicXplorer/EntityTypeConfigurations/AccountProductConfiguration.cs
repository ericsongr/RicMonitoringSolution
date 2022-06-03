using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicEntityFramework.RicXplorer.EntityTypeConfigurations
{
    public class AccountProductConfiguration : IEntityTypeConfiguration<AccountProduct>
    {
        public void Configure(EntityTypeBuilder<AccountProduct> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Name)
                .HasMaxLength(100);

            builder.HasOne(o => o.AccountProductCategory)
                .WithMany(o => o.AccountProducts)
                .HasForeignKey(o => o.AccountProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AccountProducts_AccountProductCategory");

            builder.ToTable("AccountProducts");
        }
    }
}
