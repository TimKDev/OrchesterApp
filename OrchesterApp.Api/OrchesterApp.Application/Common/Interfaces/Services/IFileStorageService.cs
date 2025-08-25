using FluentResults;
using OrchesterApp.Domain.FileStorageAggregate.ValueObjects;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Stores a file and returns the FileStorageId for referencing
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName">Original filename</param>
        /// <param name="contentType">MIME content type</param>
        /// <param name="fileStream">File data stream</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>FileStorageId that can be used as FK in other tables</returns>
        Task<Result<Guid>> StoreFileAsync(
            Guid fileId,
            string fileName,
            string contentType,
            Stream fileStream,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves multiple files and returns them as a combined stream
        /// </summary>
        /// <param name="fileStorageIds">List of FileStorageIds to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Stream containing all requested files with metadata</returns>
        Task<Result<Stream>> GetMultipleFilesStreamAsync(
            IEnumerable<Guid> fileStorageIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks a file as deleted (soft delete)
        /// </summary>
        /// <param name="fileStorageId">FileStorageId to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteFileAsync(Guid fileStorageId,
            CancellationToken cancellationToken = bad);
    }

    public record FileMetadata(
        string FileName,
        string ContentType,
        long FileSize,
        DateTime UploadDate,
        DateTime? LastAccessDate);
}