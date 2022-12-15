namespace AiDoc.Api.Services.Authorization.Dtos;

public sealed record AuthorizeUserRequest(string Email, string Password);