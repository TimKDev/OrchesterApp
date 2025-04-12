using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using OrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using System.Security.Claims;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;


namespace TvJahnOrchesterApp.Application.Features.Authorization.Endpoints
{
    public static class UpdateUserRoles
    {
        public static void MapUpdateUserRolesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/authentication/update-roles", PutUpdateUserRoles)
                .RequireAuthorization(a => a.RequireRole(RoleNames.Admin.ToString()));
        }

        private static async Task<IResult> PutUpdateUserRoles([FromBody] UpdateUserRolesCommand updateUserRolesCommand, CancellationToken cancellationToken, ISender sender)
        {
            await sender.Send(updateUserRolesCommand);
            return Results.Ok("Rollen des Users wurden erfolgreich geupdated.");
        }

        private record UpdateUserRolesCommand(string Email, string[] RoleNames) : IRequest<Unit>;

        private class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, Unit>
        {
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly UserManager<OrchesterApp.Domain.UserAggregate.User> userManager;

            public UpdateUserRolesCommandHandler(UserManager<OrchesterApp.Domain.UserAggregate.User> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
            }

            public async Task<Unit> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                //Remove existing Roles:
                var rolesOfUser = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, rolesOfUser.Where(r => !request.RoleNames.Contains(r)));

                var rolesOfUser2 = await userManager.GetRolesAsync(user);

                // Add new Roles:
                await userManager.AddToRolesAsync(user, request.RoleNames.Where(r => !rolesOfUser.Contains(r)));
                var rolesOfUser3 = await userManager.GetRolesAsync(user);

                return Unit.Value;
            }
        }
    }
}
