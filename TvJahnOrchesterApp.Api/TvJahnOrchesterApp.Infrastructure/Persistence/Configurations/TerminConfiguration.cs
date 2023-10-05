using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class TerminConfiguration : IEntityTypeConfiguration<Termin>
    {
        public void Configure(EntityTypeBuilder<Termin> builder)
        {
            ConfigureTerminTable(builder);
        }

        private void ConfigureTerminTable(EntityTypeBuilder<Termin> builder)
        {
            builder.ToTable("Termine");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("TerminId")
                .HasConversion(
                    id => id.Value,
                    value => TerminId.Create(value)
                );

            builder.HasOne<TerminArt>().WithMany().HasForeignKey(m => m.TerminArt).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<TerminStatus>().WithMany().HasForeignKey(m => m.TerminStatus).OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.OwnsOne(x => x.EinsatzPlan, ConfigureEinsatzplanTable);

            builder.OwnsMany(x => x.TerminRückmeldungOrchesterMitglieder, ConfigureTerminRückmeldungOrchesterMitgliederTable);

            builder.Metadata.FindNavigation(nameof(Termin.TerminRückmeldungOrchesterMitglieder))!.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.AbstimmungsId)
                .HasConversion(
                    id => id.Value,
                    value => AbstimmungsId.Create(value)
                );

        }

        private void ConfigureEinsatzplanTable(OwnedNavigationBuilder<Termin, EinsatzPlan> builder) {
            builder.ToTable("Einsatzpläne");
            builder.WithOwner().HasForeignKey("TerminId");
            builder.HasKey(nameof(EinsatzPlan.Id), "TerminId");
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("EinsatzplanId")
                .HasConversion(id => id.Value,
                value => EinsatzplanId.Create(value)
            );
            builder.OwnsOne(x => x.Treffpunkt);
            builder.OwnsMany(x => x.ZeitBlocks, ConfigureZeitBlock);

            builder.OwnsMany(m => m.EinsatzplanNotenMappings, einsatzplanNotenMappingBuilder =>
            {
                einsatzplanNotenMappingBuilder.ToTable("EinsatzplanNotenMapping");
                einsatzplanNotenMappingBuilder.HasKey(x => x.Id);
                einsatzplanNotenMappingBuilder.HasOne<Noten>().WithMany().HasForeignKey(m => m.NotenId);
                einsatzplanNotenMappingBuilder.WithOwner().HasForeignKey("EinsatzplanId", "TerminId");
            });

            builder.OwnsMany(m => m.EinsatzplanUniformMappings, einsatzplanUniformMappingBuilder =>
            {
                einsatzplanUniformMappingBuilder.ToTable("EinsatzplanUniformMapping");
                einsatzplanUniformMappingBuilder.HasKey(x => x.Id);
                einsatzplanUniformMappingBuilder.HasOne<Uniform>().WithMany().HasForeignKey(m => m.UniformId);
                einsatzplanUniformMappingBuilder.WithOwner().HasForeignKey("EinsatzplanId", "TerminId");
            });

            builder.Navigation(s => s.ZeitBlocks).Metadata.SetField("_zeitBlocks");
            builder.Navigation(s => s.ZeitBlocks).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.EinsatzplanNotenMappings).Metadata.SetField("_einsatzplanNotenMappings");
            builder.Navigation(s => s.EinsatzplanNotenMappings).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.EinsatzplanUniformMappings).Metadata.SetField("_einsatzplanUniformMappings");
            builder.Navigation(s => s.EinsatzplanUniformMappings).UsePropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureZeitBlock(OwnedNavigationBuilder<EinsatzPlan, ZeitBlock> builder)
        {
            builder.ToTable("Zeitblöcke");
            builder.WithOwner().HasForeignKey("EinsatzplanId", "TerminId");
            builder.HasKey(nameof(ZeitBlock.Id), "EinsatzplanId", "TerminId");
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ZeitblockId.Create(value)
                );
            builder.OwnsOne(x => x.Adresse);
        }

        private void ConfigureTerminRückmeldungOrchesterMitgliederTable(OwnedNavigationBuilder<Termin, TerminRückmeldungOrchestermitglied> builder)
        {
            builder.ToTable("TerminRückmeldungen");
            builder.WithOwner().HasForeignKey("TerminId");
            builder.HasKey(nameof(TerminRückmeldungOrchestermitglied.Id), "TerminId");
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("TerminRückmeldungsId")
                .HasConversion(
                    id => id.Value,
                    value => RückgemeldetePersonId.Create(value)
                );

            builder.Property(x => x.OrchesterMitgliedsId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrchesterMitgliedsId.Create(value)
                );

            builder.HasOne<OrchesterMitglied>().WithMany().HasForeignKey(x => x.OrchesterMitgliedsId);

            builder.Property(x => x.RückmeldungDurchAnderesOrchestermitglied)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrchesterMitgliedsId.Create(value)
                );

            builder.HasOne<OrchesterMitglied>().WithMany().HasForeignKey(x => x.RückmeldungDurchAnderesOrchestermitglied);

            builder.OwnsMany(m => m.TerminRückmeldungNotenstimmenMappings, terminRückmeldungNotenstimmeMappingBuilder =>
            {
                terminRückmeldungNotenstimmeMappingBuilder.ToTable("TerminRückmeldungNotenstimmeMapping");
                terminRückmeldungNotenstimmeMappingBuilder.HasKey(x => x.Id);
                terminRückmeldungNotenstimmeMappingBuilder.HasOne<Notenstimme>().WithMany().HasForeignKey(m => m.NotenstimmenId);
                terminRückmeldungNotenstimmeMappingBuilder.WithOwner().HasForeignKey("TerminRückmeldungsId", "TerminId");
            });


            builder.OwnsMany(m => m.TerminRückmeldungInstrumentMappings, terminRückmeldungInstrumentMappingBuilder =>
            {
                terminRückmeldungInstrumentMappingBuilder.ToTable("TerminRückmeldungInstrumentMapping");
                terminRückmeldungInstrumentMappingBuilder.HasKey(x => x.Id);
                terminRückmeldungInstrumentMappingBuilder.HasOne<Instrument>().WithMany().HasForeignKey(m => m.InstrumentId);
                terminRückmeldungInstrumentMappingBuilder.WithOwner().HasForeignKey("TerminRückmeldungsId", "TerminId");
            });

            builder.Navigation(s => s.TerminRückmeldungNotenstimmenMappings).Metadata.SetField("_terminRückmeldungNotenstimmenMappings");
            builder.Navigation(s => s.TerminRückmeldungNotenstimmenMappings).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.TerminRückmeldungInstrumentMappings).Metadata.SetField("_terminRückmeldungInstrumentMappings");
            builder.Navigation(s => s.TerminRückmeldungInstrumentMappings).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
