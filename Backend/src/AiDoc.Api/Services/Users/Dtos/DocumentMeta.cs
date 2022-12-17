using System;

namespace AiDoc.Api.Services.Users.Dtos;

public sealed record DocumentMeta
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Url { get; init; } = null!;
    public string Prediction { get; init; } = null!;
    public string[] Sentences { get; init; } = Array.Empty<string>();
}