using System;

namespace AiDoc.Api.Services.Users.Dtos;

public record User(Guid Id, string Email, string FullName);

public sealed record UserWithContent(Guid Id, string Email, string FullName, DocumentMeta[] DocumentMeta) : User(Id, Email, FullName);