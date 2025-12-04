namespace Discussion.Core.Services.R2StorageService;

using Amazon.S3;
using Amazon.S3.Model;
using DTO.Storage;
using Infrastructure.Common.Helpers;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

public class R2StorageService(IAmazonS3 s3Client, IOptions<R2Settings> options) : IR2StorageService
{
    private const int UrlExpireInMinutes = 60;

    private readonly R2Settings _options = options.Value;

    public async Task<StorageItemInternal> UploadFileAsync(
        Stream stream,
        string name,
        string? extension,
        string directory,
        bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        var extensionFromFilename = Path.GetExtension(name).Replace(".", string.Empty);
        var baseName = Path.GetFileNameWithoutExtension(name);

        var hashString = $"{baseName}-{DateTime.UtcNow.Ticks}-{Guid.NewGuid()}";
        var itemHash = CryptoHelper.Md5(hashString);
        
        string fileName;
        if (string.IsNullOrEmpty(extension))
        {
            if (!string.IsNullOrEmpty(extensionFromFilename))
            {
                fileName = $"{itemHash}.{extensionFromFilename}";
            }
            else
            {
                fileName = $"{itemHash}";
            }
        }
        else
        {
            fileName = $"{itemHash}.{extension}";
        }

        string newFileName;
        string newFileExtension;
        if (string.IsNullOrEmpty(extension))
        {
            if (string.IsNullOrEmpty(extensionFromFilename))
            {
                newFileName = $"{name}";
                newFileExtension = "";
            }
            else
            {
                newFileName = $"{baseName}.{extensionFromFilename}";
                newFileExtension = extensionFromFilename;
            }
        }
        else
        {
            newFileName = $"{baseName}.{extension}";
            newFileExtension = extension;
        }

        var path = $"{directory}/{fileName}";
        
        if (!overwrite)
        {
            if (await DoesExistAsync(path, cancellationToken))
            {
                throw new Exception();
            }
        }
        
        long fileSize = 0;
        if (stream.CanSeek)
        {
            fileSize = stream.Length;
        }
        else
        {
            throw new Exception("Handle non-seekable stream");
        }

        var putObjectRequest = new PutObjectRequest
        {
            BucketName = _options.Bucket,
            Key = path,
            InputStream = stream,
            AutoCloseStream = true,
            ContentType = "application/octet-stream",
            DisablePayloadSigning = true,
        };

        var dataObject = await s3Client.PutObjectAsync(putObjectRequest, cancellationToken);
        if (dataObject is null)
        {
            throw new Exception($"Error while uploading file {path} in " +
                                $"bucket {_options.Bucket}");
        }

        return new StorageItemInternal
        {
            BaseName = baseName,
            FileName = newFileName,
            ItemHash = itemHash,
            Extension = newFileExtension,
            FileSize = fileSize,
            StoragePath = path,
        };
    }
    
    private async Task<bool> DoesExistAsync(string path, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = _options.Bucket,
            Key = path,
        };

        try
        {
            await s3Client.GetObjectMetadataAsync(request, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception e)
        {
            if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                return false;
            throw;
        }
    }
    
    public async Task<string> SignUrl(string path)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.Bucket,
            Key = path,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(UrlExpireInMinutes)
        };

        return await s3Client.GetPreSignedURLAsync(request);
    }
}