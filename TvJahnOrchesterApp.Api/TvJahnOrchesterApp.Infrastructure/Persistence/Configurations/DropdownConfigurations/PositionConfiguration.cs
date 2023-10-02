using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => PositionId.Create(value)
                );

            builder.HasData(new[]
            {
                Position.Create((int)PositionEnum.Dirigent, "Dirigent"),
                Position.Create((int)PositionEnum.Obmann, "Obmann"),
                Position.Create((int)PositionEnum.Kassierer, "Kassierer"),
                Position.Create((int)PositionEnum.Notenwart, "Notenwart"),
                Position.Create((int)PositionEnum.Zeugwart, "Zeugwart"),
                Position.Create((int)PositionEnum.Thekenteam, "Thekenteam"),
            });
        }
    }
}
