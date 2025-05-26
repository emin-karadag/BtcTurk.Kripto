using BtcTurk.Kripto.Interfaces;
using BtcTurk.Kripto.Models.Common;
using BtcTurk.Kripto.Models.Public;
using System.Text;

namespace BtcTurk.Kripto.Services;

public class PublicService : IPublicService
{
    private readonly BtcTurkClient _client;
    public PublicService(BtcTurkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<BtcTurkBaseResponse<ExchangeInfo>?> GetExchangeInfoAsync()
    {
        var response = await _client.GetAsync<BtcTurkBaseResponse<ExchangeInfo>>("/api/v2/server/exchangeinfo");
        return response;
    }

    public async Task<BtcTurkBaseResponse<Ticker[]>?> GetTickersAsync(string? pairSymbol = null)
    {
        var endpointBuilder = new StringBuilder("/api/v2/ticker");
        if (!string.IsNullOrEmpty(pairSymbol))
            endpointBuilder.Append($"?pairSymbol={pairSymbol}");

        var response = await _client.GetAsync<BtcTurkBaseResponse<Ticker[]>>(endpointBuilder.ToString());
        return response;
    }

    public async Task<BtcTurkBaseResponse<Ticker[]>?> GetTickersByCurrencyAsync(string currency)
    {
        var response = await _client.GetAsync<BtcTurkBaseResponse<Ticker[]>>($"/api/v2/ticker/currency?symbol={currency}");
        return response;
    }

    public async Task<BtcTurkBaseResponse<OrderBook>?> GetOrderBookAsync(string pairSymbol, int? limit = null)
    {
        var endpointBuilder = new StringBuilder($"/api/v2/orderbook?pairSymbol={pairSymbol}");
        if (limit.HasValue)
            endpointBuilder.Append($"&limit={limit}");

        var response = await _client.GetAsync<BtcTurkBaseResponse<OrderBook>>(endpointBuilder.ToString());
        return response;
    }

    public async Task<BtcTurkBaseResponse<Trade[]>?> GetTradesAsync(string pairSymbol, int? last = null)
    {
        var endpointBuilder = new StringBuilder($"/api/v2/trades?pairSymbol={pairSymbol}");
        if (last.HasValue)
            endpointBuilder.Append($"&last={last.Value}");

        var response = await _client.GetAsync<BtcTurkBaseResponse<Trade[]>>(endpointBuilder.ToString());
        return response;
    }
}
