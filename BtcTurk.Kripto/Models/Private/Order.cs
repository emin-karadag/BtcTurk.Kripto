using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Private;

public class Order
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
    public decimal Quantity { get; set; }
    public decimal StopPrice { get; set; }
    public string? PairSymbol { get; set; }
    public string? PairSymbolNormalized { get; set; }
    public string? Type { get; set; }
    public string? Method { get; set; }
    public string? OrderClientId { get; set; }
    public long Time { get; set; }
    public long UpdateTime { get; set; }
    public string? Status { get; set; }
    public decimal LeftAmount { get; set; }

    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Time).DateTime;

    [JsonIgnore]
    public DateTime UpdateDateTime => DateTimeOffset.FromUnixTimeMilliseconds(UpdateTime).DateTime;
}
