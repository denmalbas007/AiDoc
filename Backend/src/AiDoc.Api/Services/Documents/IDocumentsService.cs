using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.Services.Documents.Dtos;

namespace AiDoc.Api.Services.Documents;

public interface IDocumentsService
{
    Task<AnalyzeDocumentResponse> AnalyzeAsync(AnalyzeDocumentRequest request, CancellationToken cancellationToken);
}