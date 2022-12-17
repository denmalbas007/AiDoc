using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.Infrastructure.DictCache;
using AiDoc.Api.Infrastructure.ScopeDisposer;
using AiDoc.Api.Services.Documents.Dtos;
using AiDoc.Ml.Client;
using AiDoc.Storage;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Spire.Doc;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace AiDoc.Api.Services.Documents;

public sealed class DocumentsService : IDocumentsService
{
    private readonly IScopeDisposer _disposer;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDictCache<string, int> _cache;
    private readonly MlApiClient _client;

    public DocumentsService(
        IScopeDisposer disposer,
        IHttpContextAccessor contextAccessor,
        IDictCache<string, int> cache,
        MlApiClient client)
    {
        _disposer = disposer;
        _contextAccessor = contextAccessor;
        _cache = cache;
        _client = client;
    }

    public async Task<AnalyzeDocumentResponse> AnalyzeAsync(
        AnalyzeDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var authResult = await _contextAccessor.HttpContext!.AuthenticateAsync();
        var ip = _contextAccessor.HttpContext!.Connection!.RemoteIpAddress!.ToString();
        if (!authResult.Succeeded)
        {
            // if (_cache.Get(ip) is not null)
            //     throw new ExceptionWithCode(403, "No tries left");
            _cache.Add(_contextAccessor.HttpContext!.Connection!.RemoteIpAddress!.ToString(), 1);
        }
            
        var storedFile = new StoredFile(request.FormFile);
        _disposer.AddAsync(storedFile);

        var text = storedFile.ContentType switch
        {
            MediaTypeNames.Application.Pdf => ExtractTextFromPdf(storedFile),
            "text/rtf" => ExtractTextFromRtf(storedFile),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" =>
                ExtractTextFromDocx(storedFile),
            "application/msword" => ExtractTextFromDoc(storedFile),
            _ => throw new ArgumentOutOfRangeException()
        };

        var result = await _client.GetPredictionAsync(text, cancellationToken);

        return new(result.Prediction, result.PredictionSentences);
    }

    private string ExtractTextFromPdf(StoredFile file)
    {
        using var pdf = PdfDocument.Open(file.FileStream);
        var pages = pdf.GetPages();
        var words = new List<Word>();
        foreach (var page in pages)
            words.AddRange(page.GetWords());
        var sb = new StringBuilder();
        words.ForEach(word => sb.Append(word.Text + " "));
        return sb.ToString();
    }

    private string ExtractTextFromDoc(StoredFile file)
    {
        using var document = new Document();
        document.LoadFromStream(file.FileStream, FileFormat.Doc);
        return document.GetText().Replace("Evaluation Warning: The document was created with Spire.Doc for .NET.", "");
    }


    private string ExtractTextFromDocx(StoredFile file)
    {
        using var doc = WordprocessingDocument.Open(file.FileStream, false);
        return doc.MainDocumentPart!.Document.Body!.InnerText;
    }

    private string ExtractTextFromRtf(StoredFile rtfFile)
    {
        using var document = new Document();
        document.LoadFromStream(rtfFile.FileStream, FileFormat.Rtf);
        return document.GetText().Replace("Evaluation Warning: The document was created with Spire.Doc for .NET.", "");
    }
}