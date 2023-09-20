using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific
{
    public class GetTerminByIdQueryHandler : IRequestHandler<GetTerminByIdQuery, (Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)>
    {
        private readonly ITerminRepository terminRepository;
        private readonly ICurrentUserService currentUserService;

        public GetTerminByIdQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
        {
            this.terminRepository = terminRepository;
            this.currentUserService = currentUserService;
        }

        public async Task<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.Id, cancellationToken);
            var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
            var currrentUserRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r => r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);
            if(currrentUserRückmeldung is null)
            {
                throw new Exception("Throw custom exception");
            }

            return (termin, currrentUserRückmeldung!);
        }
    }
}
