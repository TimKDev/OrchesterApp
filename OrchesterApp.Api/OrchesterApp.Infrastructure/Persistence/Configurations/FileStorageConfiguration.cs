using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrchesterApp.Infrastructure.Persistence.Configurations
{
    public class
        FileStorageConfiguration : IEntityTypeConfiguration<TvJahnOrchesterApp.Application.Common.Models.FileStorage>
    {
        public void Configure(EntityTypeBuilder<TvJahnOrchesterApp.Application.Common.Models.FileStorage> builder)
        {
            builder.ToTable("FileStorage");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedNever();

            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(f => f.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.FileSize)
                .IsRequired();

            builder.Property(f => f.StorageType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(f => f.UploadDate)
                .IsRequired();

            builder.Property(f => f.LastAccessDate);

            builder.HasOne(f => f.FileDataBytea)
                .WithOne()
                .HasForeignKey<TvJahnOrchesterApp.Application.Common.Models.FileStorage>(f => f.ByteaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}