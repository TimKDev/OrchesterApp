using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.Persistence.Configurations;

public class FileDataByteaConfiguration : IEntityTypeConfiguration<FileDataBytea>
{
    public void Configure(EntityTypeBuilder<FileDataBytea> builder)
    {
        builder.ToTable("FileDataBytea");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Data)
            .IsRequired()
            .HasColumnType("bytea");
    }
}