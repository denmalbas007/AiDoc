using System;

namespace AiDoc.Api.Services.Users.Dtos;

public sealed record User(Guid Id, string Email, string FullName);