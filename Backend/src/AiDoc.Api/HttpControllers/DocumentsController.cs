using System;
using System.Threading.Tasks;
using AiDoc.Api.Services.Documents;
using AiDoc.Api.Services.Documents.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiDoc.Api.HttpControllers;

[ApiController]
[Route("api/v1/documents")]
public sealed class DocumentsController : ControllerBase
{
    private readonly IDocumentsService _documentsService;

    public DocumentsController(IDocumentsService documentsService)
        => _documentsService = documentsService;

    [HttpPost("analyze")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Analyze(IFormFile file)
    {
        if (HttpContext.User.Identity is null && file.Length > 1000000)
            throw new Exception("Size exception");
        var request = new AnalyzeDocumentRequest(file);
        var result = await _documentsService.AnalyzeAsync(request, HttpContext.RequestAborted);
        return Ok(result);
    }
}