using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace OrchesterApp.Api
{
    public static partial class DependencyInjection
    {
        public static readonly string MyCorsPolicy = "MyCorsPolicy";

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.ConfigureCorsPolicy();
            services.AddControllers();
            services.AddLogging();

            // Configure multipart form options for file uploads
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
                options.ValueLengthLimit = int.MaxValue;
                options.ValueCountLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
            });

            services.AddOutputCache(opts =>
            {
                opts.AddBasePolicy(builder => builder.Cache());
                opts.AddPolicy("OutputCacheWithAuthPolicy", OutputCacheWithAuthPolicy.Instance);
            });

            services.AddMappings();
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly
                .GetExecutingAssembly()); // Definiert wo überall nach IRegister Klassen für die Mapster Konfigurationen gesucht werden soll

            // Registriere Mapster im DI Container 
            services.AddSingleton(config);
            services
                .AddScoped<IMapper,
                    ServiceMapper>(); // ServiceMapper ist eine erweiterte Mapperklasse, hier wird also die Mapperklasse in DI registriert
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