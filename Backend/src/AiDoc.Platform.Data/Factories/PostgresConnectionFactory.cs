using System;
using System.Data.Common;
using AiDoc.Platform.Data.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AiDoc.Platform.Data.Factories;

public sealed class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private NpgsqlConnection? _connection;
    private readonly string _connectionString;
    
    public PostgresConnectionFactory(IConfiguration configuration)
    {
        const string interfaceName = nameof(PostgresConnectionFactory);
        var connectionString = configuration[DatabaseEnvironmentConstants.DatabaseConnectionStringBase];
        if (string.IsNullOrEmpty(connectionString))
            connectionString = configuration.GetSection(interfaceName).Get<PostgresqlConnectionOptions>()?.ConnectionString;
        if (connectionString is null)
            throw new ArgumentNullException();
        var sqlStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        _connectionString = sqlStringBuilder.ToString();
    }

    public DbConnection GetConnection()
        => _connection ??= new NpgsqlConnection(_connectionString);
}
