using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.Common.Enums;

namespace OrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
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
