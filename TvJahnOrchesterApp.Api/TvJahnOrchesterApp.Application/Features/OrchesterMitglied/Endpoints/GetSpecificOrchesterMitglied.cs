using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    internal class GetSpecificOrchesterMitglied
    {
        private record GetSpecificOrchesterMitgliederResponse(Guid Id, string Vorname, string Nachname, Adresse Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int? DefaultInstrument, int? DefaultNotenStimme);
    }
}
