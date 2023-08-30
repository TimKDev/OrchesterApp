using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Infrastructure.Persistence;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories;

namespace TvJahnOrchesterApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IOrchesterMitgliedRepository, OrchesterMitgliedRepository>();
            services.AddDbContext<OrchesterDbContext>(options => options.UseSqlServer("Server=localhost;Database=BuberDinner;User Id=sa;Password=amiko123!;Encrypt=false"));
            return services;
        }
    }
}