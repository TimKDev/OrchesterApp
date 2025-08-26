using Microsoft.AspNetCore.Routing;
using OrchesterApp.Application.Features.FileStorage.Endpoints;

namespace OrchesterApp.Application.Features.FileStorage
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