using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.Common.Enums;

namespace OrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.ToTable("Instrumente");
            builder.HasKey(m => m.Id);

            builder.HasOne<ArtInstrument>().WithMany().HasForeignKey(m => m.ArtInstrumentId);

            builder.HasData(new[]
            {
                Instrument.Create((int)InstrumentEnum.Saxophon, "Saxophon", (int)ArtInstrumentEnum.Holz),
                Instrument.Create((int)InstrumentEnum.Trompete, "Trompete", (int)ArtInstrumentEnum.Blech),
                Instrument.Create((int)InstrumentEnum.Trommel, "Trommel", (int)ArtInstrumentEnum.Schlagwerk),
                Instrument.Create((int)InstrumentEnum.Dirigent, "Dirigent", (int)ArtInstrumentEnum.Dirigent)
            });
        }
    }
}
