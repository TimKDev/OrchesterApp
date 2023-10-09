using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class DeleteUser
    {
        public static void MapDeleteUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/authentication/deleteUser", DeleteDeleteUser)
                .RequireAuthorization();
        }

        private static async Task<IResult> DeleteDeleteUser([FromBody] DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(deleteUserCommand);
            return Results.Ok("User erfolgreich gelöscht");
        }

        private record DeleteUserCommand(string UserId) : IRequest<Unit>;

        private class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly UserManager<User> userManager;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public DeleteUserCommandHandler(UserManager<User> userManager, IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
            {
                this.userManager = userManager;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByUserIdAsync(request.UserId, cancellationToken);
                if (orchesterMitglied is null)
                {
                    throw new Exception("Orchestermitglied nicht gefunden");
                }

                orchesterMitglied.RemoveUser();
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var user = await userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                await userManager.DeleteAsync(user);


                return Unit.Value;
            }
        }
    }
}
