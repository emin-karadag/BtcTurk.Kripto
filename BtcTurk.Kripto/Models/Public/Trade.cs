using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Public;

public class Trade
{
    public string? Pair { get; set; }
    public string? PairNormalized { get; set; }
    public string? Numerator { get; set; }
    public string? Denominator { get; set; }
    public long Timestamp { get; set; }
    public string? Tid { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
    public string? Side { get; set; }

    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
}
