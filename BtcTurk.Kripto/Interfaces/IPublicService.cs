using BtcTurk.Kripto.Models.Common;
using BtcTurk.Kripto.Models.Public;

namespace BtcTurk.Kripto.Interfaces;

public interface IPublicService
{
    Task<BtcTurkBaseResponse<ExchangeInfo>?> GetExchangeInfoAsync();
    Task<BtcTurkBaseResponse<Ticker[]>?> GetTickersAsync(string? pairSymbol = null);
    Task<BtcTurkBaseResponse<Ticker[]>?> GetTickersByCurrencyAsync(string currency);
    Task<BtcTurkBaseResponse<OrderBook>?> GetOrderBookAsync(string pairSymbol, int? limit = null);
    Task<BtcTurkBaseResponse<Trade[]>?> GetTradesAsync(string pairSymbol, int? last = null);
}
