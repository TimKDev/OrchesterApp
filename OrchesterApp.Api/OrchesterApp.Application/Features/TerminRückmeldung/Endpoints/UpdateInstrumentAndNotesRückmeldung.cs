using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace TvJahnOrchesterApp.Application.Features.TerminRückmeldung.Endpoints
{
    public static class UpdateInstrumentAndNotesRückmeldung
    {
        public static void MapUpdateInstrumentAndNotesRückmeldungEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/termin/rückmeldung/changeInstrumentsAndNotes", PutInstrumentAndNotesRückmeldung)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin, RoleNames.Vorstand }));
        }

        private static async Task<IResult> PutInstrumentAndNotesRückmeldung([FromBody] RückmeldungChangeInstrumentsAndNotesCommand rückmeldungChangeInstrumentsAndNotesCommand, ISender sender, CancellationToken cancellationToken)
        {
            await sender.Send(rückmeldungChangeInstrumentsAndNotesCommand, cancellationToken);

            return Results.Ok("Rückmeldung wurde erfolgreich geupdated.");
        }

        private record RückmeldungChangeInstrumentsAndNotesCommand(Guid TerminId, Guid RückmeldungsId, int[] Instruments, int[] Notenstimme) : IRequest<Unit>;

        private class RückmeldungChangeInstrumentsAndNotesCommandHandler : IRequestHandler<RückmeldungChangeInstrumentsAndNotesCommand, Unit>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IUnitOfWork unitOfWork;

            public RückmeldungChangeInstrumentsAndNotesCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(RückmeldungChangeInstrumentsAndNotesCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                var rückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(e => e.Id.Value == request.RückmeldungsId);
                if (rückmeldung is null)
                {
                    throw new Exception("Füge hier eine Custom Exception ein");
                }
                //TTODO Instrumt und Notenstimme hinzufügen

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return new Unit();

            }
        }
    }
}
