namespace AiDoc.Storage;

public interface IFileStorage
{
    Task UploadFileAsync(StoredFile fileToUpload, CancellationToken cancellationToken);

    Task<StoredFile> GetFileAsync(Guid fileId, CancellationToken cancellationToken);

    Task DeleteFileAsync(Guid fileId, CancellationToken cancellationToken);
}