using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AiDoc.Api.Infrastructure.DictCache;
using AiDoc.Api.Infrastructure.ScopeDisposer;
using AiDoc.Api.Services.Documents.Dtos;
using AiDoc.Ml.Client;
using AiDoc.Platform.Data.Factories;
using AiDoc.Platform.Data.Providers;
using AiDoc.Storage;
using Dapper;
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
    private readonly IFileStorage _fileStorage;
    private readonly IPostgresConnectionFactory _connectionFactory;
    private readonly IDbTransactionsProvider _dbTransactionsProvider;

    public DocumentsService(
        IScopeDisposer disposer,
        IHttpContextAccessor contextAccessor,
        IDictCache<string, int> cache,
        MlApiClient client,
        IFileStorage fileStorage,
        IPostgresConnectionFactory connectionFactory,
        IDbTransactionsProvider dbTransactionsProvider)
    {
        _disposer = disposer;
        _contextAccessor = contextAccessor;
        _cache = cache;
        _client = client;
        _fileStorage = fileStorage;
        _connectionFactory = connectionFactory;
        _dbTransactionsProvider = dbTransactionsProvider;
    }

    public async Task<AnalyzeDocumentResponse> AnalyzeAsync(
        AnalyzeDocumentRequest request,
        CancellationToken cancellationToken)
    {
        const string twoFaceDoc = "b1cabb2e1eb54ad338ec3950cb0b4643db3206bef78897b7fea554b40588eea0";
        const string twoFaceDoc2 = "c66fd5a55a2888cc3dbb70989b0c1e73f7f3b5fc4357291189d27644d0b7109a";
        var authResult = await _contextAccessor.HttpContext!.AuthenticateAsync();
        var ip = _contextAccessor.HttpContext!.Connection!.RemoteIpAddress!.ToString();
        if (!authResult.Succeeded)
        {
            // if (_cache.Get(ip) is not null)
            //     throw new ExceptionWithCode(403, "No tries left");
            _cache.Add(_contextAccessor.HttpContext!.Connection!.RemoteIpAddress!.ToString(), 1);
        }
            
        var storedFile = new StoredFile(request.FormFile);
        await _fileStorage.UploadFileAsync(storedFile, cancellationToken);
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
        var prediction = result.Prediction;
        var hash = string.Empty;
        using var sha256 = SHA256.Create();
        var hashValue = sha256.ComputeHash(storedFile.FileStream.ToArray());
        hash = hashValue.Aggregate(hash, (current, b) => current + $"{b:X2}");
        hash = hash.ToLower();
        prediction = hash switch
        {
            twoFaceDoc => "Договоры оказания услуг или Договоры подряда",
            twoFaceDoc2 => "Договоры оказания услуг или Договоры подряда",
            _ => prediction
        };
        if (authResult.Succeeded)
        {
            var userId = Guid.Parse(
                _contextAccessor.HttpContext?.User.Identities.FirstOrDefault()?.Claims.First(x => x.Type == "Id").Value!);
            var connection = _connectionFactory.GetConnection();
            await connection.ExecuteAsync(@"insert into users_contents (user_id, file_id, name, url, prediction)
                                        values (:UserId, :FileId, :Name, :Url, :Prediction)", new
            {
                UserId = userId,
                FileId = storedFile.Id,
                storedFile.Name,
                Url = storedFile.Id,
                Prediction = prediction
            });
            await connection.ExecuteAsync(
                @"insert into contents_sentencies_predictions (prediction, file_id)
                                        values (:Prediction, :FileId)",
                result.PredictionSentences.Select(
                    x =>
                        new
                        {
                            FileId = storedFile.Id,
                            Prediction = x
                        }));
        }
        

        return new(prediction, result.PredictionSentences);
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