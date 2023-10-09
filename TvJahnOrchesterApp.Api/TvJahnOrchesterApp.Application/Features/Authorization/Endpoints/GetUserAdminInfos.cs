using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class GetUserAdminInfos
    {
        public static void MapGetUserAdminInfosEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/authentication/userAdminInfos", UserAdminInfos)
                .RequireAuthorization();
        }

        private static async Task<IResult> UserAdminInfos(CancellationToken cancellationToken, ISender sender)
        {
            var queryResult = await sender.Send(new GetUserAdminInfosQuery());
            return Results.Ok(queryResult);
        }

        private record GetUserAdminInfosQuery() : IRequest<GetUserAdminInfosResponse[]>;

        private record GetUserAdminInfosResponse(Guid OrchesterMitgliedsId, string? UserId, string RegistrationKey, DateTime RegisterKeyExpirationDate, string? Email, bool AccountLocked, DateTime? LastLogin, DateTime? FirstLogin);

        private class GetUserAdminInfosQueryHandler : IRequestHandler<GetUserAdminInfosQuery, GetUserAdminInfosResponse[]>
        {
            private IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly UserManager<User> userManager;
            public GetUserAdminInfosQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, UserManager<User> userManager)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.userManager = userManager;
            }

            public async Task<GetUserAdminInfosResponse[]> Handle(GetUserAdminInfosQuery request, CancellationToken cancellationToken)
            {
                var result = new List<GetUserAdminInfosResponse>();
                var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                foreach (var orchesterMitglied in orchesterMitglieder)
                {
                    var user = orchesterMitglied.ConnectedUserId is null ? null : await userManager.FindByIdAsync(orchesterMitglied.ConnectedUserId);
                    var userLocked = false;
                    if (user is not null)
                    {
                        userLocked = await userManager.IsLockedOutAsync(user);
                    }
                    result.Add(new GetUserAdminInfosResponse(orchesterMitglied.Id.Value, user?.Id, orchesterMitglied.RegisterKey, orchesterMitglied.RegisterKeyExpirationDate, user?.Email, userLocked, orchesterMitglied.UserLastLogin, orchesterMitglied.UserFirstConnected));
                }

                return result.ToArray();
            }
        }
    }
}
