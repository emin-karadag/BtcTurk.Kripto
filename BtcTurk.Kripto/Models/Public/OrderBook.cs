using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Public;

public class OrderBook
{
    public long Timestamp { get; set; }

    [JsonPropertyName("bids")]
    public List<decimal[]> BidsRaw { get; set; } = [];

    [JsonPropertyName("asks")]
    public List<decimal[]> AsksRaw { get; set; } = [];

    [JsonIgnore]
    public List<OrderBookEntry> Bids => [.. BidsRaw.Select(b => new OrderBookEntry
    {
        Price = b[0],
        Amount = b[1]
    })];

    [JsonIgnore]
    public List<OrderBookEntry> Asks => [.. AsksRaw.Select(a => new OrderBookEntry
    {
        Price = a[0],
        Amount = a[1]
    })];

    [JsonIgnore]
    public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;

    public class OrderBookEntry
    {
        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}
