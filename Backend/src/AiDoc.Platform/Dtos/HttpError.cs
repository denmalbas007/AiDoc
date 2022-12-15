namespace AiDoc.Platform.Dtos;

public record HttpError<T>(string ErrorMessage, T? ErrorObject = null) where T : class;

public record HttpError(string ErrorMessage, string? StackTrace = null);
