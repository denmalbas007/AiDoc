using System.Net.Http.Json;
using System.Text.Json;
using AiDoc.Ml.Client.Dtos;

namespace AiDoc.Ml.Client;

public sealed class MlApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MlApiClient(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<PredictionResponse> GetPredictionAsync(string text, CancellationToken cancellationToken)
    {
        using var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("http://109.71.9.32:80/");
        var request = new PredictionRequest() {InputString = text};
        var result = await client.PostAsJsonAsync("predict", request, cancellationToken);
        var strResult = await result.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        return JsonSerializer.Deserialize<PredictionResponse>(strResult)!;
    }
}