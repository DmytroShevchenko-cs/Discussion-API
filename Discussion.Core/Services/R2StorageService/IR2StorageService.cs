namespace Discussion.Core.Services.R2StorageService;

using DTO.Storage;

public interface IR2StorageService
{
    Task<StorageItemInternal> UploadFileAsync(
        Stream stream,
        string name,
        string? extension,
        string directory,
        bool overwrite = true,
        CancellationToken cancellationToken = default);

    Task<string> SignUrl(string path);
}