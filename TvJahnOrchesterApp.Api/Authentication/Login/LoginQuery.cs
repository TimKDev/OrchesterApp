using MediatR;
using TvJahnOrchesterApp.Application.Authentication.Common;

namespace TvJahnOrchesterApp.Application.Authentication.Queries.Login
{
    public record LoginQuery(string Email, string Password): IRequest<AuthenticationResult>;
}
