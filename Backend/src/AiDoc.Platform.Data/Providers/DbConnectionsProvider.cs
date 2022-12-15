using System;
using System.Data.Common;
using System.Threading.Tasks;
using AiDoc.Platform.Data.Factories;

namespace AiDoc.Platform.Data.Providers;

public class DbConnectionsProvider : IDbConnectionsProvider, IAsyncDisposable, IDisposable
{
    private readonly IPostgresConnectionFactory _postgresConnectionFactory;
    private DbConnection? _connection;

    public DbConnectionsProvider(IPostgresConnectionFactory postgresConnectionFactory)
        => _postgresConnectionFactory = postgresConnectionFactory;

    public DbConnection GetConnection()
    {
        _connection ??= _postgresConnectionFactory.GetConnection();
        return _connection;
    }

    public ValueTask DisposeAsync()
    {
        _connection?.Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
        => _connection?.Dispose();
}
