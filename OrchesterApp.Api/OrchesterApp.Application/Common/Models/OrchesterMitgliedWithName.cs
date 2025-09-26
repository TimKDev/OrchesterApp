using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Dto
{
    public record OrchesterMitgliedWithName(OrchesterMitgliedsId Id, string Vorname, string Nachname);
}
