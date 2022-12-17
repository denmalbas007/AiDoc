using System.Text.Json.Serialization;

namespace AiDoc.Ml.Client.Dtos;

public class PredictionRequest
{
    [JsonPropertyName("input_string")]
    public string InputString { get; init; } = null!;
}