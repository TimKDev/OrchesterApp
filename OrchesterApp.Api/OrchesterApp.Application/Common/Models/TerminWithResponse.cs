using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchesterApp.Domain.Common.Entities;
using OrchesterApp.Domain.TerminAggregate.Entities;
using OrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Dto
{
    public record TerminWithResponses(TerminId Id, int? TerminArt, IReadOnlyList<TerminRückmeldungOrchestermitglied> RückmeldungOrchestermitglieder);
}
