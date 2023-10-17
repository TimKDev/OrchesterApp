using TvJahnOrchesterApp.Api;
using TvJahnOrchesterApp.Api.Middlewares;
using TvJahnOrchesterApp.Application;
using TvJahnOrchesterApp.Application.Features;
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
                if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                app.UseMiddleware<ErrorHandelingMiddleware>();
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseCors(TvJahnOrchesterApp.Api.DependencyInjection.MyCorsPolicy);
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