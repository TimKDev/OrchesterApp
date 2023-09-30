using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class AddRegistrationKey
    {
        public static void MapAddRegistrationKeyEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Authentication/addRegistrationKey", PostAddRegistrationKey);
        }

        public static async Task<IResult> PostAddRegistrationKey([FromBody] AddRegistrationKeyCommand addRegistrationKeyCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(addRegistrationKeyCommand);
            return Results.Ok("Registration Key wurde erfolgreich hinzugefügt.");
        }

        public record AddRegistrationKeyCommand(Guid OrchesterMitgliedsId, string NewRegistrationKey) : IRequest<Unit>;

        public class AddRegistrationKeyCommandHandler : IRequestHandler<AddRegistrationKeyCommand, Unit>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public AddRegistrationKeyCommandHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(AddRegistrationKeyCommand request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId), cancellationToken);
                orchesterMitglied.SetRegisterKey(request.NewRegistrationKey);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
