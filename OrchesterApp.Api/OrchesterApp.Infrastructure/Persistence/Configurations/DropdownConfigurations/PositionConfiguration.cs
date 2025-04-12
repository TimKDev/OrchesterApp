using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.Common.Enums;

namespace OrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.HasKey(m => m.Id);

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
