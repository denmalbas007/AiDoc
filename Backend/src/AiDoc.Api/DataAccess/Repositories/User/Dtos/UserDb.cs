using System;

namespace AiDoc.Api.DataAccess.Repositories.User.Dtos;

public sealed class UserDb
{
    public string Email { get; init; } = null!;
    public Guid Id { get; init; }
    public string Password { get; init; } = null!;
    public string FullName { get; init; } = null!;
}