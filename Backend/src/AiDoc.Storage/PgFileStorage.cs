namespace AiDoc.Storage;

public sealed class PgFileStorage : IFileStorage
{
    public Task UploadFileAsync(StoredFile fileToUpload, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task<StoredFile> GetFileAsync(Guid fileId, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public Task DeleteFileAsync(Guid fileId, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}