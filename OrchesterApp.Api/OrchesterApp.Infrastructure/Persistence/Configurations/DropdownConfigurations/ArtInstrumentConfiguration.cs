﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.Common.Enums;

namespace OrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class ArtInstrumentConfiguration : IEntityTypeConfiguration<ArtInstrument>
    {
        public void Configure(EntityTypeBuilder<ArtInstrument> builder)
        {
            builder.ToTable("ArtInstrument");
            builder.HasKey(m => m.Id);
            // Seed Data
            builder.HasData(new[]
            {
                ArtInstrument.Create((int)ArtInstrumentEnum.Holz, "Holz"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Blech, "Blech"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Schlagwerk, "Schlagwerk"),
                ArtInstrument.Create((int)ArtInstrumentEnum.Dirigent, "Dirigent"),
            } );
        }
    }
}
