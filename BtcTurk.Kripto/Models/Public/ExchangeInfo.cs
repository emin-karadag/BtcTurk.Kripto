using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Models.Public;

public class ExchangeInfo
{
    public long ServerTime { get; set; }
    public string? TimeZone { get; set; }
    public List<Symbol>? Symbols { get; set; }
    public List<Currency>? Currencies { get; set; }
    public List<CurrencyOperationBlock>? CurrencyOperationBlocks { get; set; }

    [JsonIgnore]
    public DateTime ServerDateTime => DateTimeOffset.FromUnixTimeMilliseconds(ServerTime).DateTime;

    public partial class Symbol
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? NameNormalized { get; set; }
        public string? Status { get; set; }
        public string? Numerator { get; set; }
        public string? Denominator { get; set; }
        public long NumeratorScale { get; set; }
        public long DenominatorScale { get; set; }
        public bool HasFraction { get; set; }
        public List<Filter>? Filters { get; set; }
        public List<string>? OrderMethods { get; set; }
        public string? DisplayFormat { get; set; }
        public bool CommissionFromNumerator { get; set; }
        public long Order { get; set; }
        public bool PriceRounding { get; set; }
        public bool IsNew { get; set; }
        public double MarketPriceWarningThresholdPercentage { get; set; }
        public double? MaximumOrderAmount { get; set; }
        public double MaximumLimitOrderPrice { get; set; }
        public double MinimumLimitOrderPrice { get; set; }

        public partial class Filter
        {
            public string? FilterType { get; set; }
            public decimal MinPrice { get; set; }
            public decimal MaxPrice { get; set; }
            public decimal TickSize { get; set; }
            public decimal MinExchangeValue { get; set; }
            public string? MinAmount { get; set; }
            public string? MaxAmount { get; set; }
        }
    }

    public partial class Currency
    {
        public long Id { get; set; }
        public string? Symbol { get; set; }
        public decimal MinWithdrawal { get; set; }
        public decimal MinDeposit { get; set; }
        public long Precision { get; set; }
        public CurrencyAddress? Address { get; set; }
        public string? CurrencyType { get; set; }
        public CurrencyTag? Tag { get; set; }
        public string? Color { get; set; }
        public string? Name { get; set; }
        public bool IsAddressRenewable { get; set; }
        public bool GetAutoAddressDisabled { get; set; }
        public bool IsPartialWithdrawalEnabled { get; set; }
        public bool IsNew { get; set; }

        public partial class CurrencyAddress
        {
            public long? MinLen { get; set; }
            public long? MaxLen { get; set; }
        }

        public partial class CurrencyTag
        {
            public bool Enable { get; set; }
            public string? Name { get; set; }
            public long? MinLen { get; set; }
            public long? MaxLen { get; set; }
        }
    }

    public partial class CurrencyOperationBlock
    {
        public string? CurrencySymbol { get; set; }
        public bool WithdrawalDisabled { get; set; }
        public bool DepositDisabled { get; set; }
    }
}
