using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific
{
    public class GetTerminByIdQueryHandler : IRequestHandler<GetTerminByIdQuery, Domain.TerminAggregate.Termin>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        public GetTerminByIdQueryHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.terminRepository = terminRepository;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<Domain.TerminAggregate.Termin> Handle(GetTerminByIdQuery request, CancellationToken cancellationToken)
        {
            return await terminRepository.GetById(request.Id, cancellationToken);
        }
    }
}
