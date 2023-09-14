using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser
{
    internal class RückmeldungForOtherUserCommandHandler: IRequestHandler<RückmeldungForOtherUserCommand, RückmeldungsResponse>
    {
        private readonly ITerminRepository terminRepository;
        private readonly ICurrentUserService currentUserService;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        public RückmeldungForOtherUserCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository, ICurrentUserService currentUserService)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            this.currentUserService = currentUserService;
        }

        public async Task<RückmeldungsResponse> Handle(RückmeldungForOtherUserCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId), cancellationToken);
            var currentOrchestermitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);

            termin.RückmeldenZuTermin(orchesterMitglied.Id, request.Zugesagt, request.Kommentar, currentOrchestermitglied.Id);

            return new RückmeldungsResponse(orchesterMitglied, request.Zugesagt, request.Kommentar, termin.Id);
        }
    }
}
