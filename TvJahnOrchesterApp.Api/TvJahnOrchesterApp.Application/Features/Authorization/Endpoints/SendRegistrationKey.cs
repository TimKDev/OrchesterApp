using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Text;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Interfaces;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class SendRegistrationKey
    {
        public static void MapSendRegistrationKeyEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/authentication/send-registration-key", PostSendRegistrationKey)
                .RequireAuthorization(r => r.RequireRole(new string[] { RoleNames.Admin }));
        }

        private static async Task<IResult> PostSendRegistrationKey([FromBody] SendRegistrationKeyCommand addRegistrationKeyCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(addRegistrationKeyCommand);
            return Results.Ok("Registration Key wurde erfolgreich hinzugefügt.");
        }

        private record SendRegistrationKeyCommand(Guid OrchesterMitgliedsId, string Email, string ClientUri) : IRequest<Unit>;

        private class SendRegistrationKeyCommandHandler : IRequestHandler<SendRegistrationKeyCommand, Unit>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly ISendRegistrationEmailService sendRegistrationEmailService;

            public SendRegistrationKeyCommandHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork, ISendRegistrationEmailService sendRegistrationEmailService)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
                this.sendRegistrationEmailService = sendRegistrationEmailService;
            }

            public async Task<Unit> Handle(SendRegistrationKeyCommand request, CancellationToken cancellationToken)
            {
                var registrationKey = GenerateRandomString(32);

                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId), cancellationToken);

                orchesterMitglied.SetRegisterKey(registrationKey);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await sendRegistrationEmailService.SendTo(request.Email, registrationKey, request.ClientUri);

                return Unit.Value;
            }

            private static string GenerateRandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                StringBuilder stringBuilder = new StringBuilder(length);

                for (int i = 0; i < length; i++)
                {
                    int index = random.Next(chars.Length);
                    stringBuilder.Append(chars[index]);
                }

                return stringBuilder.ToString();
            }
        }
    }
}
