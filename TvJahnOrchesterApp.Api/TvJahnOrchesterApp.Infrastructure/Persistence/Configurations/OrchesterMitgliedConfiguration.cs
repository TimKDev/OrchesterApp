using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.Common.Entities;
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
            builder.Property(m => m.Id) 
                .ValueGeneratedNever()
                .HasColumnName("OrchesterMitgliedsId")
                .HasConversion(
                    id => id.Value,
                    value => OrchesterMitgliedsId.Create(value)
                );

            builder.OwnsOne(m => m.Adresse);

            builder.HasOne<Instrument>().WithMany().HasForeignKey(m => m.DefaultInstrument).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Notenstimme>().WithMany().HasForeignKey(m => m.DefaultNotenStimme).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<MitgliedsStatus>().WithMany().HasForeignKey(m => m.OrchesterMitgliedsStatus).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<User>().WithOne().HasForeignKey<OrchesterMitglied>(o => o.ConnectedUserId).OnDelete(DeleteBehavior.SetNull);

            builder.OwnsMany(m => m.PositionMappings, positionsMappingBuilder =>
            {
                positionsMappingBuilder.ToTable("OrchestermitgliedPositions");
                positionsMappingBuilder.HasKey(m => m.Id);
                positionsMappingBuilder.Property(m => m.Id)
                    .ValueGeneratedNever()
                    .HasConversion(
                        id => id.Value,
                        value => OrchesterMitgliedPositionsMappingId.Create(value)
                    );
                positionsMappingBuilder.HasOne<Position>().WithMany().HasForeignKey(m => m.PositionId);
                positionsMappingBuilder.WithOwner().HasForeignKey("OrchesterMitgliedsId");
            });

            builder.Metadata.FindNavigation(nameof(OrchesterMitglied.PositionMappings))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
