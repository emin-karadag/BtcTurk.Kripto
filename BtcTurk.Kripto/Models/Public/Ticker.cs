using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Public;

public class Ticker
{
    public string? Pair { get; set; }
    public string? PairNormalized { get; set; }
    public long Timestamp { get; set; }
    public decimal Last { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
    public decimal Open { get; set; }
    public double Volume { get; set; }
    public decimal Average { get; set; }
    public decimal Daily { get; set; }
    public decimal DailyPercent { get; set; }
    public string? DenominatorSymbol { get; set; }
    public string? NumeratorSymbol { get; set; }
    public long Order { get; set; }

    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
}
