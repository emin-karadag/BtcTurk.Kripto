using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Private;

public class SubmitOrder
{
    public long Id { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal? StopPrice { get; set; }
    public string? NewOrderClientId { get; set; }
    public string? Type { get; set; }
    public string? Method { get; set; }
    public string? PairSymbol { get; set; }
    public string? PairSymbolNormalized { get; set; }

    [JsonPropertyName("datetime")]
    public long Timestamp { get; set; }

    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
}
