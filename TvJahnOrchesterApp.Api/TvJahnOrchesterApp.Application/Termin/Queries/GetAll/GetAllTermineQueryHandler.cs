using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetAll
{
    public class GetAllTermineQueryHandler : IRequestHandler<GetAllTermineQuery, Domain.TerminAggregate.Termin[]>
    {
        private readonly ITerminRepository terminRepository;

        public GetAllTermineQueryHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<Domain.TerminAggregate.Termin[]> Handle(GetAllTermineQuery request, CancellationToken cancellationToken)
        {
            return await terminRepository.GetAll(cancellationToken);
        }
    }
}
