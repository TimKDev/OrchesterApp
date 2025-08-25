using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrchesterApp.Domain.FileStorageAggregate.ValueObjects;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace OrchesterApp.Application.Features.FileStorage.Endpoints
{
    public static class DownloadMultipleFiles
    {
        public static void MapDownloadMultipleFilesEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/files/download-multiple", HandleAsync)
                .RequireAuthorization();
        }

        public static async Task<IResult> HandleAsync(
            [FromBody] DownloadMultipleFilesRequest request,
            [FromServices] IFileStorageService fileStorageService,
            CancellationToken cancellationToken)
        {
            if (request?.FileIds == null || !request.FileIds.Any())
                return Results.BadRequest("No file IDs provided");

            if (request.FileIds.Count() > 50) // Limit to prevent abuse
                return Results.BadRequest("Too many files requested. Maximum is 50 files");

            try
            {
                var fileStorageIds = request.FileIds.Select(FileStorageId.Create);
                var combinedStream = await fileStorageService.GetMultipleFilesStreamAsync(
                    fileStorageIds,
                    cancellationToken);

                return Results.File(
                    combinedStream,
                    "application/octet-stream",
                    "combined-files.dat",
                    enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error downloading files: {ex.Message}");
            }
        }

        private record UploadFileCommand(
            Guid FileId,
            string FileName,
            string ContentType,
            Stream FileStream) : IRequest<Unit>;

        private class UploadFileCommandValidation : AbstractValidator<UploadFileCommand>
        {
            public const int FileUploadLimitInMb = 10;

            public UploadFileCommandValidation()
            {
                RuleFor(x => x.FileId).NotEmpty();
                RuleFor(x => x.FileName).NotEmpty();
                RuleFor(x => x.ContentType).NotEmpty();
                RuleFor(x => x.FileStream).NotEmpty();
                RuleFor(x => x.FileStream.Length).LessThan(10 * 1024 * 1024);
            }
        }

        private class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, Unit>
        {
            private readonly IFileStorageService _fileStorageService;

            public UploadFileCommandHandler(IFileStorageService fileStorageService)
            {
                _fileStorageService = fileStorageService;
            }

            public async Task<Unit> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                await _fileStorageService.StoreFileAsync(request.FileId, request.FileName, request.ContentType,
                    request.FileStream);

                return Unit.Value;
            }
        }
    }

    public class DownloadMultipleFilesRequest
    {
        public IEnumerable<Guid> FileIds { get; set; } = [];
    }
}