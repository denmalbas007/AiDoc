using System;

namespace AiDoc.Api.DataAccess.Repositories.User.Dtos;

public sealed record InsertUserDbCmd(
    Guid Id,
    string Email,
    string FullName,
    string PasswordHash,
    string? ProfilePicUrl);