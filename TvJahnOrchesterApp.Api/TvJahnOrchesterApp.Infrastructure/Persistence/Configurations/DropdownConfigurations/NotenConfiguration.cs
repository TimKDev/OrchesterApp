using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class NotenConfiguration : IEntityTypeConfiguration<Noten>
    {
        public void Configure(EntityTypeBuilder<Noten> builder)
        {
            builder.ToTable("Noten");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                Noten.Create((int)NotenEnum.SchwarzesMarschbuch, "Schwarzes Marschbuch"),
                Noten.Create((int)NotenEnum.BlauesMarschbuch, "Blaues Marschbuch"),
                Noten.Create((int)NotenEnum.RotesMarschbuch, "Rotes Marschbuch"),
                Noten.Create((int)NotenEnum.Konzertmappe, "Konzertmappe"),
                Noten.Create((int)NotenEnum.StMartinNoten, "St. Martin Noten"),
                Noten.Create((int)NotenEnum.KarnevalsNoten, "Karnevalsnoten"),
                Noten.Create((int)NotenEnum.Weihnachtsnoten, "Weihnachtsnoten"),
            });
        }
    }
}
