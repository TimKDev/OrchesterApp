using TvJahnOrchesterApp.Api;
using TvJahnOrchesterApp.Api.Middlewares;
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
                    .AddInfrastructure()
                    .AddApplication();
            }

            var app = builder.Build();
            {
                if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
                //app.UseMiddleware<ErrorHandelingMiddleware>();
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthorization();
                app.MapControllers();
                app.MapFallbackToFile("index.html");
                app.Run();
            }
        }
    }
}