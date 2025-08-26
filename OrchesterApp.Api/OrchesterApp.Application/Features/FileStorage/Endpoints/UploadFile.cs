using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace OrchesterApp.Application.Features.FileStorage.Endpoints;

public static class UploadFile
{
    public static void MapUploadFileEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/files/upload/{fileId}", HandleAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync([FromRoute] Guid fileId, [FromBody] IFormFile file, ISender sender,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        await sender.Send(new UploadFileCommand(fileId, file.FileName, file.ContentType, stream), cancellationToken);

        return Results.Ok("File wurde erfolgreich uploaded");
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
            RuleFor(x => x.FileStream.Length).LessThan(FileUploadLimitInMb * 1024 * 1024);
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
            var objectName = $"{request.FileId}{Path.GetExtension(request.FileName)}";

            await _fileStorageService.StoreFileAsync(objectName, request.ContentType,
                request.FileStream);

            return Unit.Value;
        }
    }
}