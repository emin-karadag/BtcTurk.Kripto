namespace BtcTurk.Kripto.Interfaces;

public interface IBtcTurkClient
{
    HttpClient HttpClient { get; }
    string BaseUrl { get; }
    bool HasCredentials { get; }
    IPublicService PublicService { get; }
    IPrivateService PrivateService { get; }
    public Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object requestBody);
    Task<T?> DeleteAsync<T>(string endpoint);
    Task<T?> SendRequestAsync<T>(HttpMethod method, string endpoint, object? requestBody = null);
}
