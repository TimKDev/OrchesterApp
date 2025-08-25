using Microsoft.EntityFrameworkCore;
using Npgsql;
using OrchesterApp.Infrastructure.Persistence;
using TvJahnOrchesterApp.Application.Common.Models;

namespace OrchesterApp.Infrastructure.FileStorage
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly OrchesterDbContext _context;

        public FileStorageRepository(OrchesterDbContext context)
        {
            _context = context;
        }

        public async Task<TvJahnOrchesterApp.Application.Common.Models.FileStorage?> GetByIdAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.FileStorages
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<TvJahnOrchesterApp.Application.Common.Models.FileStorage>> GetByIdsAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            return await _context.FileStorages
                .Where(f => ids.Contains(f.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default)
        {
            await _context.FileStorages.AddAsync(fileStorage, cancellationToken);
        }

        public async Task UpdateAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default)
        {
            _context.FileStorages.Update(fileStorage);
            await Task.CompletedTask;
        }

        public void DeleteAsync(TvJahnOrchesterApp.Application.Common.Models.FileStorage fileStorage,
            CancellationToken cancellationToken = default)
        {
            _context.FileStorages.Remove(fileStorage);
        }

        public async Task<byte[]?> GetByteaDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var fileData = await _context.FileDataByteas
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
            return fileData?.Data;
        }

        public async Task AddByteaDataAsync(FileDataBytea fileDataBytea, CancellationToken cancellationToken = default)
        {
            await _context.FileDataByteas.AddAsync(fileDataBytea, cancellationToken);
        }

        public async Task<Stream> GetLobStreamAsync(uint lobId, CancellationToken cancellationToken = default)
        {
            var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            var manager = new NpgsqlLargeObjectManager(connection);
            var stream = await manager.OpenReadAsync(lobId, cancellationToken);
            return stream;
        }

        public async Task<uint> CreateLobAsync(Stream dataStream, CancellationToken cancellationToken = default)
        {
            var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            var manager = new NpgsqlLargeObjectManager(connection);
            var lobId = await manager.CreateAsync(0, cancellationToken);

            await using var lobStream = await manager.OpenReadWriteAsync(lobId, cancellationToken);
            await dataStream.CopyToAsync(lobStream, cancellationToken);

            return lobId;
        }

        public async Task DeleteLobAsync(uint lobId, CancellationToken cancellationToken = default)
        {
            var connection = (NpgsqlConnection)_context.Database.GetDbConnection();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            var manager = new NpgsqlLargeObjectManager(connection);
            await manager.UnlinkAsync(lobId, cancellationToken);
        }
    }
}