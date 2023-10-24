using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class GetUserAdminInfos
    {
        public static void MapGetUserAdminInfosEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/authentication/user-admin-infos", UserAdminInfos)
                .RequireAuthorization(r => r.RequireRole(new string[] {RoleNames.Admin}));
        }

        private static async Task<IResult> UserAdminInfos(CancellationToken cancellationToken, ISender sender)
        {
            var queryResult = await sender.Send(new GetUserAdminInfosQuery());
            return Results.Ok(queryResult);
        }

        private record GetUserAdminInfosQuery() : IRequest<GetUserAdminInfosResponse[]>;

        private record GetUserAdminInfosResponse(Guid OrchesterMitgliedsId, string? UserId, string RegistrationKey, DateTime RegisterKeyExpirationDate, string? Email, bool AccountLocked, DateTime? LastLogin, DateTime? FirstLogin, string[] RoleNames, string OrchesterMitgliedsName);

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
                    string[] userRoles = Array.Empty<string>();
                    if (user is not null)
                    {
                        userLocked = await userManager.IsLockedOutAsync(user);
                        userRoles = (await userManager.GetRolesAsync(user)).ToArray();
                    }
                    result.Add(new GetUserAdminInfosResponse(orchesterMitglied.Id.Value, user?.Id, orchesterMitglied.RegisterKey, orchesterMitglied.RegisterKeyExpirationDate, user?.Email, userLocked, orchesterMitglied.UserLastLogin, orchesterMitglied.UserFirstConnected, userRoles, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}"));
                }

                return result.ToArray();
            }
        }
    }
}
