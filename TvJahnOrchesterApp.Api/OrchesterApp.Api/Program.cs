using TvJahnOrchesterApp.Api;
using TvJahnOrchesterApp.Infrastructure.Persistence;
using TvJahnOrchesterApp.Application.Features;
using TvJahnOrchesterApp.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Application;
using TvJahnOrchesterApp.Infrastructure;
using Microsoft.Extensions.Configuration;
using TvJahnOrchesterApp.Infrastructure.Services;

namespace OrchesterApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);
                builder.Services
                    .AddPresentation()
                    .AddInfrastructure(builder.Configuration)
                    .AddApplication();
            }

            var app = builder.Build();
            {
                using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var initDataBaseService = serviceScope.ServiceProvider.GetRequiredService<InitDatabaseService>();
                    initDataBaseService.OnStartUp().Wait();
                }

                if (!app.Environment.IsDevelopment())
                {
                    app.UseHsts();
                }
                app.UseMiddleware<ErrorHandelingMiddleware>();

                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors(TvJahnOrchesterApp.Api.DependencyInjection.MyCorsPolicy);
                app.UseOutputCache();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.RegisterEndpointsFeatures();
                app.MapFallbackToFile("/");
                app.Run();
            }
        }
    }
}
