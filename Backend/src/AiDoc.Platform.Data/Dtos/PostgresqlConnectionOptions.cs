using System.ComponentModel.DataAnnotations;

namespace AiDoc.Platform.Data.Dtos;

public sealed class PostgresqlConnectionOptions
{
    [Required]
    public string ConnectionString { get; init; } = null!;
}
