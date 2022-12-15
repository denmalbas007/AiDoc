using Microsoft.AspNetCore.Http;

namespace AiDoc.Api.Services.Documents.Dtos;

public sealed record AnalyzeDocumentRequest(IFormFile FormFile);