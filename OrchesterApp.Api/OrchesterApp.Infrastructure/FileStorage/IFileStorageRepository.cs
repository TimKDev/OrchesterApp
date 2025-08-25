using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.FileStorage
{
    internal interface IFileStorageRepository
    {
        Task<TvJahnOrchesterApp.Application.Common.Models.FileStorage?> GetByIdAsync(Guid id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<TvJahnOrchesterApp.Application.Common.Models.FileStorage>> GetByIdsAsync(IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default);

        Task AddAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default);

        void DeleteAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default);

        Task<byte[]?> GetByteaDataAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddByteaDataAsync(FileDataBytea fileDataBytea, CancellationToken cancellationToken = default);
        Task<Stream> GetLobStreamAsync(uint lobId, CancellationToken cancellationToken = default);
        Task<uint> CreateLobAsync(Stream dataStream, CancellationToken cancellationToken = default);
        Task DeleteLobAsync(uint lobId, CancellationToken cancellationToken = default);
    }
}