using MediatR;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit
{
    public record UpdateAnwesenheitCommand(Guid TerminId, UpdateAnwesenheitsEintrag[] UpdateAnwesenheitsListe): IRequest<UpdateAnwesenheitsListeResponse>;
}
