using System.Text;
using FluentResults;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;
using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.FileStorage
{
    internal class FileStorageService : IFileStorageService
    {
        private const long LargeFileSizeThreshold = 1024 * 1024; // 1MB
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FileStorageService(
            IFileStorageRepository fileStorageRepository,
            IUnitOfWork unitOfWork)
        {
            _fileStorageRepository = fileStorageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> StoreFileAsync(Guid fileId, string fileName, string contentType,
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            if (!fileStream.CanSeek || fileStream.Length > 0)
            {
                throw new ArgumentException(nameof(fileStream));
            }

            if (fileStream.Length > LargeFileSizeThreshold)
            {
                return await StoreLargeFileAsync(fileId, fileName, contentType, fileStream, cancellationToken);
            }

            return await StoreSmallFileAsync(fileId, fileName, contentType, fileStream, cancellationToken);
        }


        public async Task<Result<Stream>> GetMultipleFilesStreamAsync(
            IEnumerable<Guid> fileStorageIds,
            CancellationToken cancellationToken = default)
        {
            var files = await _fileStorageRepository.GetByIdsAsync(fileStorageIds, cancellationToken);
            var combinedStream = new MemoryStream();

            foreach (var file in files)
            {
                try
                {
                    await AddFileToStreamAsync(combinedStream, file, cancellationToken);
                }
                catch (Exception)
                {
                    // Log the error and continue with next file
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            combinedStream.Position = 0;
            return combinedStream;
        }

        private async Task AddFileToStreamAsync(MemoryStream combinedStream,
            TvJahnOrchesterApp.Application.Common.Models.FileStorage file,
            CancellationToken cancellationToken)
        {
            file.LastAccessDate = DateTime.UtcNow;
            await _fileStorageRepository.UpdateAsync(file, cancellationToken);

            var fileStream = file.StorageType switch
            {
                FileStorageType.LargeObject when file.LargeObjectId.HasValue =>
                    await _fileStorageRepository.GetLobStreamAsync(file.LargeObjectId.Value,
                        cancellationToken),
                FileStorageType.Bytea when file.ByteaId.HasValue =>
                    new MemoryStream(
                        await _fileStorageRepository.GetByteaDataAsync(file.Id, cancellationToken) ?? []),
                _ => new MemoryStream([])
            };

            var header = CreateFileHeader(file);
            await combinedStream.WriteAsync(header, cancellationToken);
            await fileStream.CopyToAsync(combinedStream, cancellationToken);
            await fileStream.DisposeAsync();

            file.LastAccessDate = DateTime.UtcNow;
            await _fileStorageRepository.UpdateAsync(file, cancellationToken);
        }

        public async Task DeleteFileAsync(
            Guid fileStorageId,
            CancellationToken cancellationToken = default)
        {
            var file = await _fileStorageRepository.GetByIdAsync(fileStorageId, cancellationToken);
            if (file == null)
            {
                return;
            }

            if (file.StorageType == FileStorageType.LargeObject && file.LargeObjectId.HasValue)
            {
                await _fileStorageRepository.DeleteLobAsync(file.LargeObjectId.Value, cancellationToken);
            }

            _fileStorageRepository.DeleteAsync(file, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Guid> StoreLargeFileAsync(Guid fileId, string fileName,
            string contentType,
            Stream fileStream,
            CancellationToken cancellationToken)
        {
            var lobId = await _fileStorageRepository.CreateLobAsync(fileStream, cancellationToken);

            var fileStorage = new TvJahnOrchesterApp.Application.Common.Models.FileStorage()
            {
                Id = fileId,
                FileName = fileName,
                ContentType = contentType,
                FileSize = fileStream.Length,
                ByteaId = null,
                StorageType = FileStorageType.LargeObject,
                UploadDate = DateTime.UtcNow,
                LargeObjectId = lobId,
                LastAccessDate = null,
            };

            await _fileStorageRepository.AddAsync(fileStorage, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return fileStorage.Id;
        }

        private async Task<Result<Guid>> StoreSmallFileAsync(Guid fileId, string fileName, string contentType,
            Stream fileStream,
            CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            var fileData = memoryStream.ToArray();

            var bytea = new FileDataBytea()
            {
                Id = Guid.NewGuid(),
                Data = fileData,
            };

            await _fileStorageRepository.AddByteaDataAsync(bytea, cancellationToken);

            var fileStorage = new TvJahnOrchesterApp.Application.Common.Models.FileStorage()
            {
                Id = fileId,
                FileName = fileName,
                ContentType = contentType,
                ByteaId = bytea.Id,
                FileSize = fileData.Length,
                StorageType = FileStorageType.Bytea,
                LargeObjectId = null,
                UploadDate = DateTime.UtcNow,
                LastAccessDate = null,
            };

            await _fileStorageRepository.AddAsync(fileStorage, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return fileStorage.Id;
        }

        private static byte[] CreateFileHeader(TvJahnOrchesterApp.Application.Common.Models.FileStorage file)
        {
            var headerInfo = $"FILE_START|{file.FileName}|{file.ContentType}|{file.FileSize}\n";
            return Encoding.UTF8.GetBytes(headerInfo);
        }
    }
}