using System;
using System.Threading.Tasks;
using AiDoc.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AiDoc.Api.HttpControllers;

[ApiController]
[Route("api/v1/files")]
public sealed class FilesController : ControllerBase
{
    private readonly IFileStorage _fileStorage;

    public FilesController(IFileStorage fileStorage)
        => _fileStorage = fileStorage;

    [HttpGet("{fileId:guid}")]
    public async Task<IActionResult> GetFileById(Guid fileId)
    {
        var result = await _fileStorage.GetFileAsync(fileId, HttpContext.RequestAborted);
        return File(result.FileStream, result.ContentType, fileDownloadName: result.Name);
    }
}