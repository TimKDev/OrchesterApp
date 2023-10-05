using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class NotenstimmenConfiguration : IEntityTypeConfiguration<Notenstimme>
    {
        public void Configure(EntityTypeBuilder<Notenstimme> builder)
        {
            builder.ToTable("Notenstimme");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                Notenstimme.Create((int)NotenstimmeEnum.AltSaxophon1, "Alt Saxophon 1"),
                Notenstimme.Create((int)NotenstimmeEnum.AltSaxophon2, "Alt Saxophon 2"),
                Notenstimme.Create((int)NotenstimmeEnum.SopranSaxophon, "Sopran Saxophon"),
                Notenstimme.Create((int)NotenstimmeEnum.Trompete1, "Trompete 1"),
                Notenstimme.Create((int)NotenstimmeEnum.Trompete2, "Trompete 2"),
                Notenstimme.Create((int)NotenstimmeEnum.Trompete3, "Trompete 3"),
                Notenstimme.Create((int)NotenstimmeEnum.Schlagzeug, "Schlagzeug"),
            });
        }
    }
}