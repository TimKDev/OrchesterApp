using TvJahnOrchesterApp.Api;
using TvJahnOrchesterApp.Infrastructure.Persistence;
using TvJahnOrchesterApp.Application.Features;
using TvJahnOrchesterApp.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Application;
using TvJahnOrchesterApp.Infrastructure;

namespace OrchesterApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services
                    .AddPresentation()
                    .AddInfrastructure(builder.Configuration)
                    .AddApplication();
            }

            var app = builder.Build();
            {
                using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<OrchesterDbContext>();
                    context.Database.Migrate();
                }

                if (!app.Environment.IsDevelopment())
                {
                    app.UseHsts();
                }
                app.UseMiddleware<ErrorHandelingMiddleware>();
 
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors(TvJahnOrchesterApp.Api.DependencyInjection.MyCorsPolicy);
                app.UseOutputCache();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.RegisterEndpointsFeatures();
                app.MapFallbackToFile("index.html");
                app.Run();
            }
        }
    }
}
