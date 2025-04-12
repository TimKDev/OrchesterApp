using TvJahnOrchesterApp.Application.Features;
using TvJahnOrchesterApp.Application;
using OrchesterApp.Infrastructure;
using OrchesterApp.Api.Middlewares;
using OrchesterApp.Infrastructure.Services;

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
                app.UseCors(DependencyInjection.MyCorsPolicy);
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