using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace TvJahnOrchesterApp.Application.Features.FileStorage.Endpoints
{
    public static class DownloadFile
    {
        public static void MapDownloadFileEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/files/download/{objectName}", HandleAsync)
                .RequireAuthorization();
        }

        private static async Task<IResult> HandleAsync([FromRoute] string objectName,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(new DownloadFileQuery(objectName), cancellationToken);

            return result is not null
                ? Results.Stream(
                    stream: result.FileStream,
                    contentType: result.ContentType,
                    fileDownloadName: result.FileName,
                    enableRangeProcessing: true)
                : Results.NotFound("File wurde nicht gefunden.");
        }

        private record DownloadFileQuery(string ObjectName) : IRequest<GetFileResult?>;

        private class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
        {
            public DownloadFileQueryValidator()
            {
                RuleFor(x => x.ObjectName).NotEmpty();
            }
        }

        private class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, GetFileResult?>
        {
            private readonly IFileStorageService _fileStorageService;

            public DownloadFileQueryHandler(IFileStorageService fileStorageService)
            {
                _fileStorageService = fileStorageService;
            }

            public Task<GetFileResult?> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
            {
                return _fileStorageService.GetFileAsync(request.ObjectName, cancellationToken);
            }
        }
    }
}