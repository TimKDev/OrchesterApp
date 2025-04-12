using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrchesterApp.Domain.UserAggregate;
using OrchesterApp.Infrastructure.Authentication;
using OrchesterApp.Infrastructure.Email;
using OrchesterApp.Infrastructure.Persistence;
using OrchesterApp.Infrastructure.Persistence.Repositories;
using OrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories;
using OrchesterApp.Infrastructure.Services;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using OrchesterApp.Infrastructure.Extensions;

namespace OrchesterApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddPersistence(configuration)
                .AddEmail(configuration)
                .AddAuthentication(configuration);

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetValueFromSecretOrConfig("ConnectionStrings:DefaultConnection");
            services.AddDbContext<OrchesterDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IOrchesterMitgliedRepository, OrchesterMitgliedRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITerminRepository, TerminRepository>();
            services.AddScoped<IInstrumentRepository, InstrumentRepository>();
            services.AddScoped<IDropdownRepository, InstrumentRepository>();
            services.AddScoped<IDropdownRepository, NotenstimmeRepository>();
            services.AddScoped<IDropdownRepository, MitgliedsStatusRepository>();
            services.AddScoped<IDropdownRepository, PositionRepository>();
            services.AddScoped<IDropdownRepository, NotenRepository>();
            services.AddScoped<IDropdownRepository, UniformRepository>();
            services.AddScoped<IDropdownRepository, TerminArtenRepository>();
            services.AddScoped<IDropdownRepository, RückmeldungsArtenRepository>();
            services.AddScoped<IDropdownRepository, TerminStatusRepository>();

            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, ConfigurationManager configuration)
        {
            var emailConfig = new EmailConfiguration();

            configuration.Bind(EmailConfiguration.SectionName, emailConfig);
            emailConfig.Password = configuration.GetValueFromSecretOrConfig("Google_Mail_Api")!;

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<InitDatabaseService>();

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                // Hier können Optionen für die Authentication mit Identity gesetzt werden:
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<OrchesterDbContext>()
            .AddDefaultTokenProviders()
            ;

            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            var secretValue = configuration.GetValueFromSecretOrConfig("JWT_Secret_Key");
            jwtSettings.Secret = secretValue!;

            services.AddSingleton(Options.Create(jwtSettings));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(secretValue!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}