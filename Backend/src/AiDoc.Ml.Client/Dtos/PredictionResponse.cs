using System.Text.Json.Serialization;

namespace AiDoc.Ml.Client.Dtos;

public class PredictionResponse
{
    [JsonPropertyName("prediction")]
    public string Prediction { get; set; } = null!;

    [JsonPropertyName("prediction_sentences")]
    public string[] PredictionSentences { get; set; } = null!;
}