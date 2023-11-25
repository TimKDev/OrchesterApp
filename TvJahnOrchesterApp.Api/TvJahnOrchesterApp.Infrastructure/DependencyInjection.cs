using BuberDinner.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Infrastructure.Authentication;
using TvJahnOrchesterApp.Infrastructure.Common.Interfaces;
using TvJahnOrchesterApp.Infrastructure.Email;
using TvJahnOrchesterApp.Infrastructure.Persistence;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories;
using TvJahnOrchesterApp.Infrastructure.Persistence.Repositories.DropdownRepositories;

namespace TvJahnOrchesterApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddPersistence()
                .AddEmail(configuration)
                .AddAuthentication(configuration);

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<OrchesterDbContext>(options => options.UseSqlServer("Server=localhost;Database=OrchesterAppDB;User Id=sa;Password=amiko123!;Encrypt=false"));

            services.AddScoped<IOrchesterMitgliedRepository, OrchesterMitgliedRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITerminRepository, TerminRepository>();
            services.AddScoped<IInstrumentRepository, InstrumentRepository>();
            services.AddScoped<IDropdownRepository, InstrumentRepository>();
            services.AddScoped<IDropdownRepository, NotenstimmeRepository>();
            services.AddScoped<IDropdownRepository, MitgliedsStatusRepository>();
            services.AddScoped<IDropdownRepository, PositionRepository>();

            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, ConfigurationManager configuration)
        {
            var emailConfig = new EmailConfiguration();
            configuration.Bind(EmailConfiguration.SectionName, emailConfig);

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();

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
                        .GetBytes(jwtSettings.Secret)),
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