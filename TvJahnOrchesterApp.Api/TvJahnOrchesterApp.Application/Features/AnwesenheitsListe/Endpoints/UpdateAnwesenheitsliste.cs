using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.AnwesenheitsListe.Endpoints
{
    public static class UpdateAnwesenheitsListe
    {
        public static void MapUpdateAnwesenheitsListeEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/Termin/anwesenheit/{terminId}", UpdateAnwesenheitTermin)
                .RequireAuthorization();
        }

        public static async Task<IResult> UpdateAnwesenheitTermin(UpdateAnwesenheitCommand updateAnwesenheitCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(updateAnwesenheitCommand, cancellationToken);
            return Results.Ok("Anwesenheitsliste wurde erfolgreich geupdated.");
        }

        public record UpdateAnwesenheitCommand(Guid TerminId, UpdateAnwesenheitsEintrag[] UpdateAnwesenheitsListe) : IRequest<Unit>;

        internal class UpdateAnwesenheitCommandHandler : IRequestHandler<UpdateAnwesenheitCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public UpdateAnwesenheitCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(UpdateAnwesenheitCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                foreach (var anwesenheitsElement in request.UpdateAnwesenheitsListe)
                {
                    var terminRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(e => e.OrchesterMitgliedsId == OrchesterMitgliedsId.Create(anwesenheitsElement.OrchesterMitgliedsId));
                    if (terminRückmeldung is null)
                    {
                        continue;
                    }
                    terminRückmeldung.ChangeAnwesenheit(anwesenheitsElement.Anwesend, anwesenheitsElement.Kommentar);
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}
