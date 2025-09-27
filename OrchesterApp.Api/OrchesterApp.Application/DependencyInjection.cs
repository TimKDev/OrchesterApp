using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TvJahnOrchesterApp.Application.Common;
using TvJahnOrchesterApp.Application.Common.Behaviors;
using TvJahnOrchesterApp.Application.Features.Authorization;
using TvJahnOrchesterApp.Application.Features.Dropdown;
using TvJahnOrchesterApp.Application.Features.Notification;
using TvJahnOrchesterApp.Application.Features.Termin;

namespace TvJahnOrchesterApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAuthorizationFeature();
            services.AddDropdownFeature();
            services.AddNotificationFeature();
            services.AddCommon();

            return services;
        }
    }
}