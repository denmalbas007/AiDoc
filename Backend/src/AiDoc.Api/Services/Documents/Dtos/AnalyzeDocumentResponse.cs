namespace AiDoc.Api.Services.Documents.Dtos;

public sealed record AnalyzeDocumentResponse(string Prediction, string[] Sentences);