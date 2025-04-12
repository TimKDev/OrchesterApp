using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OrchesterApp.Domain.OrchesterMitgliedAggregate;
using OrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.Authorization.Models;

namespace OrchesterApp.Infrastructure.Authentication
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<User> userManager;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

        private User cachedCurrentUser = null!;
        private OrchesterMitglied cachedCurrentOrchesterMitglied = null!;

        public CurrentUserService(IOrchesterMitgliedRepository orchesterMitgliedRepository, UserManager<User> userManager, IHttpContextAccessor httpContext)
        {
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            this.userManager = userManager;
            this.httpContext = httpContext;
        }

        public async Task<OrchesterMitglied> GetCurrentOrchesterMitgliedAsync(CancellationToken cancellationToken)
        {
            if(cachedCurrentOrchesterMitglied is null)
            {
                var orchesterMitgliedsId = (await GetCurrentUserAsync(cancellationToken)).OrchesterMitgliedsId;
                cachedCurrentOrchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(orchesterMitgliedsId, cancellationToken);
            }
            return cachedCurrentOrchesterMitglied;
        }

        public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            if(cachedCurrentUser is null)
            {
                var userClaim = httpContext.HttpContext?.User ?? throw new Exception();
                var currentUserId = userClaim.Claims.FirstOrDefault(c => c.Type == "Id")!.Value;
                cachedCurrentUser = await userManager.FindByIdAsync(currentUserId) ?? throw new Exception();
            }
            return cachedCurrentUser;
        }

        public async Task<bool> IsUserVorstand(CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);
            return await userManager.IsInRoleAsync(currentUser, RoleNames.Vorstand.ToString());
        }
    }
}
