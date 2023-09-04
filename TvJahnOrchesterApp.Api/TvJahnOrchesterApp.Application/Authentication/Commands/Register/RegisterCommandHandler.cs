using MediatR;
using TvJahnOrchesterApp.Application.Authentication.Common;

namespace TvJahnOrchesterApp.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        Task<AuthenticationResult> IRequestHandler<RegisterCommand, AuthenticationResult>.Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
