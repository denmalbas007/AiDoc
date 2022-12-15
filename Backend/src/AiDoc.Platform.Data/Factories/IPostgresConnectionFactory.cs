using System.Data.Common;

namespace AiDoc.Platform.Data.Factories;

public interface IPostgresConnectionFactory
{
    DbConnection GetConnection();
}
