using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Private;

public class AccountBalance
{
    public string? Asset { get; set; }
    public string? Assetname { get; set; }
    public decimal Balance { get; set; }
    public decimal Locked { get; set; }
    public decimal Free { get; set; }
    public decimal OrderFund { get; set; }
    public decimal RequestFund { get; set; }
    public int Precision { get; set; }
    public long Timestamp { get; set; }

    [JsonIgnore]
    public DateTime TimestampDateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
}
