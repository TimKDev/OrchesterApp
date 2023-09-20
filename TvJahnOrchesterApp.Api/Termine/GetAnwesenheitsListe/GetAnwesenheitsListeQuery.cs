using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAnwesenheitsListe
{
    public record GetAnwesenheitsListeQuery(Guid TerminId): IRequest<TerminAnwesenheitsListeResponse>;
}
