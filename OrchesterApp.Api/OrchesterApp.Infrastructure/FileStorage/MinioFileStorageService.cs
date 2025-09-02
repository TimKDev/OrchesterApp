using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using TvJahnOrchesterApp.Application.Common.Interfaces.Services;

namespace OrchesterApp.Infrastructure.FileStorage;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioSettings _settings;

    public MinioFileStorageService(IMinioClient minioClient, IOptions<MinioSettings> settings)
    {
        _minioClient = minioClient;
        _settings = settings.Value;
    }

    public async Task StoreFileAsync(string objectName, string contentType, Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
    }

    public async Task<GetFileResult?> GetFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var stream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName)
                .WithCallbackStream(s => s.CopyTo(stream));

            await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

            var statObjectArgs = new StatObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName);

            var objectStat = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
            stream.Position = 0;

            return new GetFileResult(objectStat.ObjectName, objectStat.ContentType, stream);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(_settings.BucketName);

        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

        if (found)
        {
            return;
        }

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(_settings.BucketName);

        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }
}