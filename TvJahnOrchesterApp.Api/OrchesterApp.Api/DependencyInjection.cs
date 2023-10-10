using Mapster;
using MapsterMapper;
using System.Reflection;

namespace TvJahnOrchesterApp.Api
{
    public static class DependencyInjection
    {
        public static readonly string MyCorsPolicy = "MyCorsPolicy";
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {

            services.ConfigureCorsPolicy();
            services.AddControllers();
            services.AddMappings();
            services.AddEndpointsApiExplorer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly()); // Definiert wo überall nach IRegister Klassen für die Mapster Konfigurationen gesucht werden soll

            // Registriere Mapster im DI Container 
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>(); // ServiceMapper ist eine erweiterte Mapperklasse, hier wird also die Mapperklasse in DI registriert
            return services;
        }

        public static IServiceCollection ConfigureCorsPolicy(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(MyCorsPolicy,
                                      policy =>
                                      {
                                          policy.AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                                      });
            });
        }
    }
}
