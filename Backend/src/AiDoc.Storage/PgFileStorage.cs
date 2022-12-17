namespace AiDoc.Storage;

public sealed class PgFileStorage : IFileStorage
{
    private readonly IFileStoragePgRepository _repository;

    public PgFileStorage(IFileStoragePgRepository repository)
        => _repository = repository;

    public Task UploadFileAsync(StoredFile fileToUpload, CancellationToken cancellationToken)
        => _repository.InsertFileAsync(fileToUpload, cancellationToken);

    public Task<StoredFile> GetFileAsync(Guid fileId, CancellationToken cancellationToken)
        => _repository.SelectFileAsync(fileId, cancellationToken);

    public Task DeleteFileAsync(Guid fileId, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}