using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.AbstimmungsAggregate.ValueObjects;
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

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.OwnsOne(x => x.EinsatzPlan, einsatzPlanBuilder => ConfigureEinsatzplanTable(einsatzPlanBuilder));

            builder.OwnsMany(x => x.TerminRückmeldungOrchesterMitglieder, terminRückmeldungOrchesterMitgliederBuilder => ConfigureTerminRückmeldungOrchesterMitgliederTable(terminRückmeldungOrchesterMitgliederBuilder));

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
            builder.OwnsMany(x => x.ZeitBlocks, zeitBlockBuilder => ConfigureZeitBlock(zeitBlockBuilder));

            builder.OwnsMany(x => x.Noten, notenBuilder =>
            {
                notenBuilder.ToTable("EinsatzplanNoten");
                notenBuilder.WithOwner().HasForeignKey("EinsatzplanId", "TerminId");
            });

            builder.OwnsMany(x => x.Uniform, uniformBuilder =>
            {
                uniformBuilder.ToTable("EinsatzplanUniform");
                uniformBuilder.WithOwner().HasForeignKey("EinsatzplanId", "TerminId");
            });

            builder.Navigation(s => s.ZeitBlocks).Metadata.SetField("_zeitBlocks");
            builder.Navigation(s => s.ZeitBlocks).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.Noten).Metadata.SetField("_noten");
            builder.Navigation(s => s.Noten).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.Uniform).Metadata.SetField("_uniform");
            builder.Navigation(s => s.Uniform).UsePropertyAccessMode(PropertyAccessMode.Field);
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

            builder.Property(x => x.RückmeldungDurchAnderesOrchestermitglied)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrchesterMitgliedsId.Create(value)
                );

            builder.OwnsMany(x => x.Instruments, instrumentBuilder =>
            {
                instrumentBuilder.ToTable("RückmeldungInstruments");
                instrumentBuilder.WithOwner().HasForeignKey("TerminRückmeldungsId", "TerminId");
            });

            builder.OwnsMany(x => x.Notenstimme, notenStimmenBuilder =>
            {
                notenStimmenBuilder.ToTable("RückmeldungNotenstimmen");
                notenStimmenBuilder.WithOwner().HasForeignKey("TerminRückmeldungsId", "TerminId");
            });

            builder.Navigation(s => s.Notenstimme).Metadata.SetField("_notenstimmen");
            builder.Navigation(s => s.Notenstimme).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(s => s.Instruments).Metadata.SetField("_instruments");
            builder.Navigation(s => s.Instruments).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
