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

        private record GetUserAdminInfosResponse(Guid OrchesterMitgliedsId, string? UserId, string? Email, DateTime? LastLogin, string OrchesterMitgliedsName);

        private record GetUserAdminInfosQuery() : IRequest<GetUserAdminInfosResponse[]>;

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
                var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAdminInfo(cancellationToken);
                foreach (var orchesterMitglied in orchesterMitglieder)
                {
                    var user = orchesterMitglied.ConnectedUserId is null ? null : await userManager.FindByIdAsync(orchesterMitglied.ConnectedUserId);
                    result.Add(new GetUserAdminInfosResponse(orchesterMitglied.Id.Value, user?.Id, user?.Email, orchesterMitglied.UserLastLogin, $"{orchesterMitglied.Vorname} {orchesterMitglied.Nachname}"));
                }

                return result.ToArray();
            }
        }
    }
}
