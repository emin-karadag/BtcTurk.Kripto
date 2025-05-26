namespace BtcTurk.Kripto.Models.Common;

public class BtcTurkBaseResponse<T> where T : class
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Code { get; set; }
}
