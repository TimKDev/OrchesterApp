using Mapster;
using TvJahnOrchesterApp.Application.Authentication.Commands.Register;
using TvJahnOrchesterApp.Contracts.Authentication;

namespace TvJahnOrchesterApp.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();
        }
    }
}
