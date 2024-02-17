using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Application.Features.Authorization.Models.Errors;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class RegisterUser
    {
        public static void MapRegisterUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/register", PostRegisterUser);
        }

        private static async Task<IResult> PostRegisterUser([FromBody] RegisterUserCommand registerUserCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(registerUserCommand);
            return Results.Ok("Registrierung war erfolgreich.");
        }

        private record RegisterUserCommand(string RegisterationKey, string Email, string Password, string ClientUri) : IRequest<Unit>;

        private class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
        {
            public RegisterUserCommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        private class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly ITokenService tokenService;

            public RegisterUserCommandHandler(UserManager<User> userManager, IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork, ITokenService tokenService)
            {
                this.userManager = userManager;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                this.tokenService = tokenService;
            }

            //TTODO: Methode in kleinere Methoden unterteilen (Clean Code). Versuchen möglichst viel Logik in die Domäne zu verlegen.
            public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByRegistrationKeyAsync(request.RegisterationKey, cancellationToken);

                if (orchesterMitglied is null)
                {
                    throw new InvalidRegistrationKeyException();
                }

                if (!orchesterMitglied.ValidateRegistrationKey(request.RegisterationKey))
                {
                    throw new InvalidRegistrationKeyException();
                }

                var user = User.Create(orchesterMitglied.Id, request.Email);
                user.UserName = request.Email;
                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    throw new IdentityRegistrationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var createdUser = await userManager.FindByEmailAsync(request.Email);
                createdUser!.EmailConfirmed = true;
                orchesterMitglied.ConnectWithUser(createdUser!.Id);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                //var token = await tokenService.GenerateAccessTokenAsync(createdUser);
                //var refreshToken = tokenService.GenerateRefreshToken();

                //await tokenService.AddRefreshTokenToUserInDBAsync(user, refreshToken);

                //await userManager.ResetAccessFailedCountAsync(user);

                return Unit.Value;
            }
        }
    }
}
