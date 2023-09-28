using Mapster;
using MapsterMapper;
using System.Reflection;

namespace TvJahnOrchesterApp.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddMappings();
            services.AddEndpointsApiExplorer();
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
    }
}
