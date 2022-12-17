using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Providers;
using Dapper;

namespace AiDoc.Storage;

public class FileStoragePgRepository : IFileStoragePgRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;
    private readonly IDbTransactionsProvider _transactionsProvider;

    public FileStoragePgRepository(
        IPostgresConnectionFactory connectionFactory,
        IDbTransactionsProvider transactionsProvider)
    {
        _connectionFactory = connectionFactory;
        _transactionsProvider = transactionsProvider;
    }


    public Task InsertFileAsync(StoredFile storedFile, CancellationToken cancellationToken)
    {
        const string query = @"insert into storage 
                               (id, name, url, content_disposition, content_type, bytes)
                               values (:Id, :Name, :Url, :ContentDisposition, :ContentType, :Bytes)";

        var connection = _connectionFactory.GetConnection();
        var param = new
        {
            storedFile.Id,
            storedFile.Name,
            storedFile.ContentDisposition,
            storedFile.Url,
            storedFile.ContentType,
            Bytes = storedFile.GetBase64String()
        };
        return connection.ExecuteAsync(
            query,
            param,
            _transactionsProvider.Current,
            commandTimeout: 30);
    }

    public async Task<StoredFile> SelectFileAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query = @"select * from storage where id = :Id;";

        var connection = _connectionFactory.GetConnection();
        var param = new {Id = id};
        var result = await connection.QueryFirstOrDefaultAsync<StoredFileDb>(
            query,
            param,
            _transactionsProvider.Current,
            commandTimeout: 30);

        return new StoredFile(result.Id, result.Name, result.ContentDisposition, result.ContentType, result.Bytes);
    }
}