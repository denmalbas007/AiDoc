using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace AiDoc.Platform.Data.Providers;

public interface IDbTransactionsProvider
{
    DbTransaction? Current { get; }

    Task<DbTransaction> BeginTransactionAsync(CancellationToken token);

    Task<DbTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken token);
}
