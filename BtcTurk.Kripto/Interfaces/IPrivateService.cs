using BtcTurk.Kripto.Models.Common;
using BtcTurk.Kripto.Models.Private;

namespace BtcTurk.Kripto.Interfaces;

public interface IPrivateService
{
    Task<BtcTurkBaseResponse<AccountBalance[]>?> GetBalancesAsync();
    Task<BtcTurkBaseResponse<SubmitOrder>?> SubmitOrderAsync(string symbol, decimal price, decimal quantity, string side, string type, decimal? stopPrice = null, string? newOrderClientId = null);
    Task<BtcTurkBaseResponse<object>?> CancelOrderAsync(long orderId);
    Task<BtcTurkBaseResponse<Order>?> GetOrderAsync(long orderId);
    Task<BtcTurkBaseResponse<Order[]>?> GetAllOrdersAsync(string symbol, int? limit = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? orderId = null, int? page = null);
    Task<BtcTurkBaseResponse<Orders>?> GetOpenOrdersAsync(string? symbol = null);
}
