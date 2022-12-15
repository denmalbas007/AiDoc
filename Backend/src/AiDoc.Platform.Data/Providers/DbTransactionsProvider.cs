using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace AiDoc.Platform.Data.Providers;

public class DbTransactionsProvider : IDbTransactionsProvider, IAsyncDisposable, IDisposable
{
    private readonly IDbConnectionsProvider _connectionsProvider;

    public DbTransactionsProvider(IDbConnectionsProvider connectionsProvider)
        => _connectionsProvider = connectionsProvider;

    public DbTransaction? Current { get; private set; }

    public Task<DbTransaction> BeginTransactionAsync(CancellationToken token)
        => BeginTransactionAsync(IsolationLevel.Unspecified, token);

    public async Task<DbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken token)
    {
        var connection = _connectionsProvider.GetConnection();
        if (!connection.State.HasFlag(ConnectionState.Open))
            await connection.OpenAsync(token);

        Current = await connection.BeginTransactionAsync(isolationLevel, token);
        return Current;
    }

    public ValueTask DisposeAsync()
        => Current?.DisposeAsync() ?? ValueTask.CompletedTask;

    public void Dispose()
        => Current?.Dispose();
}
