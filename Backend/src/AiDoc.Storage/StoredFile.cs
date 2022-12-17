using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace AiDoc.Storage;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class StoredFile : IDisposable, IAsyncDisposable
{
    public StoredFile(IFormFile formFile)
    {
        Id = Guid.NewGuid();
        Name = formFile.FileName;
        ContentDisposition = formFile.ContentDisposition;
        ContentType = formFile.ContentType;
        var memStream = new MemoryStream();
        formFile.CopyTo(memStream);
        memStream.Position = 0;
        FileStream = memStream;
    }

    public StoredFile(string url, string name, string contentDisposition, string contentType, MemoryStream fileStream)
    {
        Id = Guid.NewGuid();
        Url = url;
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        FileStream = fileStream;
    }
    
    public StoredFile(Guid id, string url, string name, string contentDisposition, string contentType, MemoryStream fileStream)
    {
        Id = id;
        Url = url;
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        FileStream = fileStream;
    }
    
    public StoredFile(Guid id, string url, string name, string contentDisposition, string contentType, string base64)
    {
        Id = id;
        Url = url;
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        var stream = new MemoryStream(Convert.FromBase64String(base64));
        FileStream = stream;
    }
    
    public StoredFile(string name, string contentDisposition, string contentType, MemoryStream fileStream)
    {
        Id = Guid.NewGuid();
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        FileStream = fileStream;
    }
    
    public StoredFile(Guid id, string name, string contentDisposition, string contentType, MemoryStream fileStream)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        FileStream = fileStream;
    }
    
    public StoredFile(Guid id, string name, string contentDisposition, string contentType, string base64)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        var stream = new MemoryStream(Convert.FromBase64String(base64));
        FileStream = stream;
    }

    public Guid Id { get; }

    public string? Url { get; init; }

    public string Name { get; } = null!;

    public string ContentDisposition { get; } = null!;

    public string ContentType { get; } = null!;

    public MemoryStream FileStream { get; } = new();

    public ReadOnlySpan<byte> GetBytes()
        => new(FileStream.ToArray());

    public string GetBase64String()
        => Convert.ToBase64String(FileStream.ToArray());

    public void Dispose()
        => FileStream.Dispose();

    public ValueTask DisposeAsync()
        => FileStream.DisposeAsync();
}