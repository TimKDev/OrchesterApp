using MediatR;
using Microsoft.AspNetCore.Identity;
using TvJahnOrchesterApp.Application.Authentication.Common;
using TvJahnOrchesterApp.Application.Authentication.Common.Errors;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Authentication.Queries.Login
{
    internal class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public LoginQueryHandler(
            UserManager<User> userManager,
            ITokenService tokenService 
        )
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if(await userManager.FindByEmailAsync(request.Email) is not User user)
            {
                throw new InvalidCredentialsException();
            }
            if(!await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new InvalidCredentialsException();
            }
            
            var token = await tokenService.GenerateAccessTokenAsync(user);

            return new AuthenticationResult(user, token);
        }
    }
}
