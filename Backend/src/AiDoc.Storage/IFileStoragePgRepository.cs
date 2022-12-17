namespace AiDoc.Storage;

public interface IFileStoragePgRepository
{
    // Плохой код.
    Task InsertFileAsync(StoredFile storedFile, CancellationToken cancellationToken);

    Task<StoredFile> SelectFileAsync(Guid id, CancellationToken cancellationToken);
}