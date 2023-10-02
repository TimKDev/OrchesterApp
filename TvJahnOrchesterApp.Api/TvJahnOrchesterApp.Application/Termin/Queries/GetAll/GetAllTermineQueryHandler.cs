using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAll
{
    public class GetAllTermineQueryHandler : IRequestHandler<GetAllTermineQuery, (Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied?)[]>
    {
        private readonly ITerminRepository terminRepository;
        private readonly ICurrentUserService currentUserService;

        public GetAllTermineQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
        {
            this.terminRepository = terminRepository;
            this.currentUserService = currentUserService;
        }

        public async Task<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)[]> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
        {
            var result = new List<(Domain.TerminAggregate.Termin, TerminRückmeldungOrchestermitglied)>();
            var termins =  await terminRepository.GetAll(cancellationToken);
            foreach ( var termin in termins )
            {
                var currentOrchesterMitglied = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var currrentUserRückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(r => r.OrchesterMitgliedsId == currentOrchesterMitglied.Id);
                result.Add((termin, currrentUserRückmeldung ?? TerminRückmeldungOrchestermitglied.Create(OrchesterMitgliedsId.CreateUnique(), new List<InstrumentId>(), new List<NotenstimmeId>())));
            }

            return result.ToArray();
        }
    }
}
