using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAllAnwesenheitsListe
{
    public record GetAllAnwesenheitsListeQuery(): IRequest<GlobalAnwesenheitsListe>;
}
