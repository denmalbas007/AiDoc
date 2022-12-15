using System.Data.Common;
using System.Linq;
using AiDoc.Platform.Data.Dtos;
using AiDoc.Platform.Data.Factories.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AiDoc.Platform.Data.Factories;

public sealed class PostgresConnectionFactory<T> 
    : IPostgresConnectionFactory<T> 
    where T : ICustomConnectionFactory
{
    private NpgsqlConnection? _connection;
    private readonly string _connectionString;
    
    public PostgresConnectionFactory(IConfiguration configuration)
    {
        var interfaceName = GetType().GenericTypeArguments.First().Name.Remove(0,1);
        var nameForEnvironment = interfaceName.Replace("ConnectionFactory", "").ToUpper();
        var environmentKey = DatabaseEnvironmentConstants.DatabaseConnectionStringBase + "_" + nameForEnvironment;
        var connectionString = configuration[environmentKey];
        if (string.IsNullOrEmpty(connectionString))
            connectionString = configuration.GetValue<PostgresqlConnectionOptions>(interfaceName)?.ConnectionString;
        var sqlStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        _connectionString = sqlStringBuilder.ToString();
    }

    public DbConnection GetConnection()
        => _connection ??= new NpgsqlConnection(_connectionString);
}