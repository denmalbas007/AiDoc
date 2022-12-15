namespace AiDoc.Api.Services.Authorization.Dtos;

public sealed record RegisterNewUserRequest(string Email, string FullName, string Password, string? ProfilePicUrl);