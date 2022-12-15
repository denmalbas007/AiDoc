using System.Data.Common;

namespace AiDoc.Platform.Data.Providers;

public interface IDbConnectionsProvider
{
    DbConnection GetConnection();
}
