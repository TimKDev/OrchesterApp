using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;

namespace TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung
{
    public class RückmeldungCommandHandler : IRequestHandler<RückmeldungCommand, RückmeldungsResponse>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ITerminRepository terminRepository;
        private readonly IUnitOfWork unitOfWork;

        public RückmeldungCommandHandler(ICurrentUserService currentUserService, ITerminRepository terminRepository, IUnitOfWork unitOfWork)
        {
            this.currentUserService = currentUserService;
            this.terminRepository = terminRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<RückmeldungsResponse> Handle(RückmeldungCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
            termin.RückmeldenZuTermin(currentOrchesterMitglied.Id, request.Zugesagt, request.Kommentar);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new RückmeldungsResponse(currentOrchesterMitglied, request.Zugesagt, request.Kommentar, termin.Id);
        }
    }
}
