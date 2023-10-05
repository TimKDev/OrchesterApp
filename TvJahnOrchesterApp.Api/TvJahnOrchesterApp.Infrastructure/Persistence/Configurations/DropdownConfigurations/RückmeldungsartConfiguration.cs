using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class RückmeldungsartConfiguration : IEntityTypeConfiguration<Rückmeldungsart>
    {
        public void Configure(EntityTypeBuilder<Rückmeldungsart> builder)
        {
            builder.ToTable("Rückmeldungsarten");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                Rückmeldungsart.Create((int)RückmeldungsartEnum.NichtZurückgemeldet, "Nicht Zurückgemeldet"),
                Rückmeldungsart.Create((int)RückmeldungsartEnum.Zugesagt, "Zugesagt"),
                Rückmeldungsart.Create((int)RückmeldungsartEnum.Abgesagt, "Abgesagt"),
            });
        }
    }
}
