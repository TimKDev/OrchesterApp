using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class OrchesterMitgliedConfiguration : IEntityTypeConfiguration<OrchesterMitglied>
    {
        public void Configure(EntityTypeBuilder<OrchesterMitglied> builder)
        {
            ConfigureOrchesterMitgliedsTable(builder);
        }
        

        private void ConfigureOrchesterMitgliedsTable(EntityTypeBuilder<OrchesterMitglied> builder)
        {
            builder.ToTable("Orchestermitglieder");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id) // Es soll ja direkt der Wert der OrchesterMitgliedsId als PK verwendet werden und nicht noch eine extra Tabelle für die ValueObjects OrchesterMitgliedsId erstellt werden!
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrchesterMitgliedsId.Create(value)
                );

            builder.OwnsOne(m => m.Adresse); // Dies erzeugt eine Owned Entity Adresse mit dem Owner Orchestermitglied, d.h. die Spalten von Adresse werden den Spalten der Orchestermitgliedstabelle hinzugefügt => Dadurch bekommt man das gewünschte Verhalten, dass die Owned Property gelöscht wird, wenn der Owner gelöscht wird. Adresse benötigt in diesem Fall keinen PK.

            builder.OwnsOne(m => m.DefaultInstrument);
            builder.OwnsOne(m => m.DefaultNotenStimme);
            builder.OwnsOne(m => m.OrchesterMitgliedsStatus);
            builder.HasOne<User>().WithOne().HasForeignKey<OrchesterMitglied>(o => o.ConnectedUserId);

            builder.Ignore(m => m.Positions);
            builder.Ignore(m => m.TerminRückmeldungen);
            builder.Ignore(m => m.AusgeliehendesOrchesterEigentum);


        }
    }
}
