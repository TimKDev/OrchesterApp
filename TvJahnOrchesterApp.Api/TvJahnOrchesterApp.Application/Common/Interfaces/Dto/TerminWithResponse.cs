using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using TvJahnOrchesterApp.Domain.TerminAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Dto
{
    public record TerminWithResponses(TerminId Id, IReadOnlyList<TerminRückmeldungOrchestermitglied> RückmeldungOrchestermitglieder);
}
