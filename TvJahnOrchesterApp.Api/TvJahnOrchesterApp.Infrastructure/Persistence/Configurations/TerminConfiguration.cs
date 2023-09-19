using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvJahnOrchesterApp.Domain.TerminAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations
{
    internal class TerminConfiguration : IEntityTypeConfiguration<Termin>
    {
        public void Configure(EntityTypeBuilder<Termin> builder)
        {
        }
    }
}
