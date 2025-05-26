using BtcTurk.Kripto.Interfaces;
using BtcTurk.Kripto.Models.Common;
using BtcTurk.Kripto.Models.Private;
using System.Text;

namespace BtcTurk.Kripto.Services;

public class PrivateService : IPrivateService
{
    private readonly BtcTurkClient _client;
    public PrivateService(BtcTurkClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));

        if (!_client.HasCredentials)
            throw new ArgumentException("API credentials must be provided for private service operations.");
    }

    public async Task<BtcTurkBaseResponse<AccountBalance[]>?> GetBalancesAsync()
    {
        var response = await _client.GetAsync<BtcTurkBaseResponse<AccountBalance[]>>("/api/v1/users/balances");
        return response;
    }

    public async Task<BtcTurkBaseResponse<SubmitOrder>?> SubmitOrderAsync(string symbol, decimal price, decimal quantity, string side, string type, decimal? stopPrice = null, string? newOrderClientId = null)
    {
        var parameters = new Dictionary<string, object>
        {
            { "pairSymbol", symbol},
            { "orderType", side },
            { "orderMethod", type },
            { "price", price },
            { "quantity", quantity }
        };

        if (stopPrice.HasValue)
            parameters.Add("stopPrice", stopPrice.Value);

        if (!string.IsNullOrEmpty(newOrderClientId))
            parameters.Add("newOrderClientId", newOrderClientId);

        var response = await _client.PostAsync<BtcTurkBaseResponse<SubmitOrder>>("/api/v1/order", parameters);
        return response;
    }

    public async Task<BtcTurkBaseResponse<object>?> CancelOrderAsync(long orderId)
    {
        var response = await _client.DeleteAsync<BtcTurkBaseResponse<object>>($"/api/v1/order?id={orderId}");
        return response;
    }

    public async Task<BtcTurkBaseResponse<Order>?> GetOrderAsync(long orderId)
    {
        var response = await _client.GetAsync<BtcTurkBaseResponse<Order>>($"/api/v1/order/{orderId}");
        return response;
    }

    public async Task<BtcTurkBaseResponse<Order[]>?> GetAllOrdersAsync(string symbol, int? limit = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? orderId = null, int? page = null)
    {
        var endpointBuilder = new StringBuilder($"/api/v1/allOrders?pairSymbol={symbol}");

        if (limit.HasValue)
            endpointBuilder.Append($"&limit={limit.Value}");

        if (startDate.HasValue)
            endpointBuilder.Append($"&startTime={startDate.Value.ToUnixTimeSeconds()}");

        if (endDate.HasValue)
            endpointBuilder.Append($"&endTime={endDate.Value.ToUnixTimeSeconds()}");

        if (!string.IsNullOrEmpty(orderId))
            endpointBuilder.Append($"&orderId={orderId}");

        if (page.HasValue)
            endpointBuilder.Append($"&page={page.Value}");

        var result = await _client.GetAsync<BtcTurkBaseResponse<Order[]>>(endpointBuilder.ToString());
        return result;
    }

    public async Task<BtcTurkBaseResponse<Orders>?> GetOpenOrdersAsync(string? symbol = null)
    {
        string endpoint = "/api/v1/openOrders";
        if (!string.IsNullOrEmpty(symbol))
            endpoint += $"?pairSymbol={symbol}";

        var result = await _client.GetAsync<BtcTurkBaseResponse<Orders>>(endpoint);
        return result;
    }
}
