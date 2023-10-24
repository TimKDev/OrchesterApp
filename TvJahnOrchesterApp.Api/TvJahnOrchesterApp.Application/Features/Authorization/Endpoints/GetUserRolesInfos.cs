using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class GetUserRolesInfos
    {
        public static void MapGetUserRolesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/authentication/user-roles", QueryUserRoles)
                .RequireAuthorization();
        }

        private static async Task<IResult> QueryUserRoles(CancellationToken cancellationToken, ISender sender)
        {
            var queryResult = await sender.Send(new GetUserAdminInfosQuery());
            return Results.Ok(queryResult);
        }

        private record GetUserAdminInfosQuery() : IRequest<GetUserRolesResponse>;

        private record GetUserRolesResponse(string[] RoleNames);

        private class GetUserAdminInfosQueryHandler : IRequestHandler<GetUserAdminInfosQuery, GetUserRolesResponse>
        {
            private readonly IHttpContextAccessor httpContextAccessor;
            public GetUserAdminInfosQueryHandler(IHttpContextAccessor httpContextAccessor)
            {
                this.httpContextAccessor = httpContextAccessor;
            }

            public async Task<GetUserRolesResponse> Handle(GetUserAdminInfosQuery request, CancellationToken cancellationToken)
            {

                var userRoles = httpContextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();

                return new GetUserRolesResponse(userRoles);
                
            }
        }
    }
}
