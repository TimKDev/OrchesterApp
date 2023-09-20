using Mapster;
using TvJahnOrchesterApp.Application.Authentication.Commands.Register;
using TvJahnOrchesterApp.Application.Authentication.Common;
using TvJahnOrchesterApp.Application.Authentication.Queries.Login;
using TvJahnOrchesterApp.Contracts.Authentication;

namespace TvJahnOrchesterApp.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();

            config.NewConfig<AuthenticationResult, RegisterResponse>()
                .Map(dest => dest, src => src.User);

            config.NewConfig<LoginRequest, LoginQuery>();

            config.NewConfig<AuthenticationResult, LoginResponse>()
                .Map(dest => dest, src => src.User);
        }
    }
}
