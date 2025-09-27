using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace TvJahnOrchesterApp.Application.Features.FileStorage.Endpoints;

public static class UploadFile
{
    public static void MapUploadFileEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/files/upload/{fileName}", HandleAsync)
            .RequireAuthorization()
            .Accepts<IFormFile>("multipart/form-data")
            .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync([FromRoute] string fileName, IFormFile file, ISender sender,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        await sender.Send(new UploadFileCommand(fileName, file.ContentType, stream), cancellationToken);

        return Results.Ok("File wurde erfolgreich uploaded");
    }

    private record UploadFileCommand(
        string FileName,
        string ContentType,
        Stream FileStream) : IRequest<Unit>;

    private class UploadFileCommandValidation : AbstractValidator<UploadFileCommand>
    {
        public const int FileUploadLimitInMb = 10;

        public UploadFileCommandValidation()
        {
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
            await _fileStorageService.StoreFileAsync(request.FileName, request.ContentType,
                request.FileStream);

            return Unit.Value;
        }
    }
}