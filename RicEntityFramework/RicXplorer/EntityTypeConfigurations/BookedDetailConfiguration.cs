using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RicModel.RicXplorer;

namespace RicMonitoringAPI.RicXplorer.Entities.EntityTypeConfig
{
    public class BookedDetailConfiguration : IEntityTypeConfiguration<BookedDetail>
    {
        public void Configure(EntityTypeBuilder<BookedDetail> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .Property(t => t.Country)
                .HasMaxLength(100);

            builder
                .Property(t => t.LanguagesSpoken)
                .HasMaxLength(100);

            builder
                .Property(t => t.Email)
                .HasMaxLength(50);

            builder
                .Property(t => t.Contact)
                .HasMaxLength(15);

            builder
                .Property(t => t.LeaveMessage)
                .HasMaxLength(1000);

            builder.ToTable("BookedDetails");
        }
    }
}
