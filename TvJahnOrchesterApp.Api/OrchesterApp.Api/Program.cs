using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvJahnOrchesterApp.Api;
using TvJahnOrchesterApp.Api.Middlewares;
using TvJahnOrchesterApp.Application;
using TvJahnOrchesterApp.Application.Features;
using TvJahnOrchesterApp.Application.Features.Dropdown;
using TvJahnOrchesterApp.Application.Features.Dropdown.Models;
using TvJahnOrchesterApp.Infrastructure;
using TvJahnOrchesterApp.Infrastructure.Persistence;

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
                app.UseHsts();
                //app.UseMiddleware<ErrorHandelingMiddleware>();
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors(TvJahnOrchesterApp.Api.DependencyInjection.MyCorsPolicy);
                app.UseOutputCache();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.RegisterEndpointsFeatures();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapFallbackToFile("index.html");
                app.Run();
            }

            
        }
    }
}