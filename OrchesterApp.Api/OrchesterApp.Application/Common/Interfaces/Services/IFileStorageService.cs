namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileStream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StoreFileAsync(string objectName, string contentType, Stream fileStream,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetFileResult?> GetFileAsync(string objectName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);
    }
}