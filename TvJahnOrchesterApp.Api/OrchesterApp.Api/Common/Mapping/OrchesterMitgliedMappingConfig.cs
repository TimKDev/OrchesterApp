using Mapster;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate;

namespace TvJahnOrchesterApp.Api.Common.Mapping
{
    public class OrchesterMitgliedMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateOrchesterMitgliedRequest, CreateOrchesterMitgliedCommand>();
            config.NewConfig<OrchesterMitglied, CreateOrchesterMitgliedResponse>();
        }
    }
}
