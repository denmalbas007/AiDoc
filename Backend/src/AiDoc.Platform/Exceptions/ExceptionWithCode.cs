using System;

namespace AiDoc.Platform.Exceptions;

public class ExceptionWithCode : Exception
{
    public ExceptionWithCode(int httpCode, string message)
        : base(message)
        => HttpCode = httpCode;

    public ExceptionWithCode(int httpCode, string message, Exception exception)
        : base(message, exception)
        => HttpCode = httpCode;

    public int HttpCode { get; }
}
