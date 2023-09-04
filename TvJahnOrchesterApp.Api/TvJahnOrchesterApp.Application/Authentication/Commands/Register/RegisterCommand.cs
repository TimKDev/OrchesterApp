using MediatR;
using TvJahnOrchesterApp.Application.Authentication.Common;

namespace TvJahnOrchesterApp.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string RegisterationKey, string Email, string Password, string ClientUri) : IRequest<AuthenticationResult>;
}
