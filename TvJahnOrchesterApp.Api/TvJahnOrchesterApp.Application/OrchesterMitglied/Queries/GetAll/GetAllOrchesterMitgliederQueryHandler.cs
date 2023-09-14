using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

namespace TvJahnOrchesterApp.Application.OrchesterMitglied.Queries.GetAll
{
    internal class GetAllOrchesterMitgliederQueryHandler : IRequestHandler<GetAllOrchesterMitgliederQuery, Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]>
    {
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        public GetAllOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
        {
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
        }

        public async Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> Handle(GetAllOrchesterMitgliederQuery request, CancellationToken cancellationToken)
        {
            var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
            return orchesterMitglieder;
        }
    }
}
