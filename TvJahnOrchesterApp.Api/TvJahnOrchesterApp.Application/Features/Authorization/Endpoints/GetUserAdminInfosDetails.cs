using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class GetUserAdminInfosDetails
    {
        public static void MapGetUserAdminInfosDetailsEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/authentication/user-admin-infos-details/{orchesterMitgliedsId}", UserAdminInfosDetails)
                .RequireAuthorization(r => r.RequireRole(new string[] {RoleNames.Admin}));
        }

        private static async Task<IResult> UserAdminInfosDetails(Guid orchesterMitgliedsId, CancellationToken cancellationToken, ISender sender)
        {
            var queryResult = await sender.Send(new GetUserAdminInfosDetailsQuery(orchesterMitgliedsId));
            return Results.Ok(queryResult);
        }

        private record GetUserAdminInfosResponseDetails(Guid OrchesterMitgliedsId, string? UserId, string RegistrationKey, DateTime RegisterKeyExpirationDate, string? Email, bool AccountLocked, DateTime? LastLogin, DateTime? FirstLogin, string[] RoleNames, string OrchesterMitgliedsName);

        private record GetUserAdminInfosDetailsQuery(Guid OrchesterMitgliedsId) : IRequest<GetUserAdminInfosResponseDetails>;

        private class GetUserAdminInfosQueryHandler : IRequestHandler<GetUserAdminInfosDetailsQuery, GetUserAdminInfosResponseDetails>
        {
            private IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly UserManager<User> userManager;
            public GetUserAdminInfosQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, UserManager<User> userManager)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.userManager = userManager;
            }

            public async Task<GetUserAdminInfosResponseDetails> Handle(GetUserAdminInfosDetailsQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId), cancellationToken);
                var user = orchesterMitglied.ConnectedUserId is null ? null : await userManager.FindByIdAsync(orchesterMitglied.ConnectedUserId);
                var userLocked = false;
                string[] userRoles = Array.Empty<string>();
                if (user is not null)
                {
                    userLocked = await userManager.IsLockedOutAsync(user);
                    userRoles = (await userManager.GetRolesAsync(user)).ToArray();
                }
                return new GetUserAdminInfosResponseDetails(orchesterMitglied.Id.Value, user?.Id, orchesterMitglied.RegisterKey, orchesterMitglied.RegisterKeyExpirationDate, user?.Email, userLocked, orchesterMitglied.UserLastLogin, orchesterMitglied.UserFirstConnected, userRoles, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}");
            }
        }
    }
}
