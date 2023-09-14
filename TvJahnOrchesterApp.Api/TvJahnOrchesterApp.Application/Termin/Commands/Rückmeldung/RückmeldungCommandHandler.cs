using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung
{
    public class RückmeldungCommandHandler : IRequestHandler<RückmeldungCommand, RückmeldungsResponse>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ITerminRepository terminRepository;

        public RückmeldungCommandHandler(ICurrentUserService currentUserService, ITerminRepository terminRepository)
        {
            this.currentUserService = currentUserService;
            this.terminRepository = terminRepository;
        }

        public async Task<RückmeldungsResponse> Handle(RückmeldungCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
            termin.RückmeldenZuTermin(currentOrchesterMitglied.Id, request.Zugesagt, request.Kommentar);

            return new RückmeldungsResponse(currentOrchesterMitglied, request.Zugesagt, request.Kommentar, termin.Id);
        }
    }
}
