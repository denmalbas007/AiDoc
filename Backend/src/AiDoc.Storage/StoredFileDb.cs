namespace AiDoc.Storage;

public class StoredFileDb
{
    public Guid Id { get; init; }

    public string? Url { get; init; }

    public string Name { get; init;  } = null!;

    public string ContentDisposition { get; init; } = null!;

    public string ContentType { get; init; } = null!;

    public string Bytes { get; init; } = null!;
}