using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Features.FileStorage.Endpoints;

namespace TvJahnOrchesterApp.Application.Features.FileStorage
{
    public static class RegisterEndpoints
    {
        public static IEndpointRouteBuilder RegisterFileStorageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapUploadFileEndpoint();
            app.MapDownloadFileEndpoint();

            return app;
        }
    }
}