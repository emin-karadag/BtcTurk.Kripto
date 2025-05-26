using BtcTurk.Kripto.Core.Helpers;
using BtcTurk.Kripto.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BtcTurk.Kripto.Services;

public class BtcTurkClient : IBtcTurkClient
{
    private const string ApiBaseUrl = "https://api.btcturk.com";
    private readonly string? _apiKey;
    private readonly string? _secretKey;
    private readonly Lazy<IPublicService> _publicService;
    private readonly Lazy<IPrivateService> _privateService;

    public HttpClient HttpClient { get; }
    public string BaseUrl => ApiBaseUrl;
    public bool HasCredentials => !string.IsNullOrWhiteSpace(_apiKey) && !string.IsNullOrWhiteSpace(_secretKey);

    public IPublicService PublicService => _publicService.Value;
    public IPrivateService PrivateService => _privateService.Value;

    public BtcTurkClient()
    {
        HttpClient = new HttpClient();
        _apiKey = null;
        _secretKey = null;

        _publicService = new Lazy<IPublicService>(() => new PublicService(this));
        _privateService = new Lazy<IPrivateService>(() => new PrivateService(this));

        ConfigureHttpClient();
    }

    public BtcTurkClient(string apiKey, string secretKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("Secret key cannot be null or empty.", nameof(secretKey));

        HttpClient = new HttpClient();
        _apiKey = apiKey;
        _secretKey = secretKey;

        _publicService = new Lazy<IPublicService>(() => new PublicService(this));
        _privateService = new Lazy<IPrivateService>(() => new PrivateService(this));

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        HttpClient.BaseAddress = new Uri(ApiBaseUrl);
        HttpClient.DefaultRequestHeaders.Accept.Clear();
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<T?> SendRequestAsync<T>(HttpMethod method, string endpoint, object? requestBody = null)
    {
        using var request = new HttpRequestMessage(method, endpoint);

        if (requestBody != null)
        {
            var jsonBody = JsonSerializer.Serialize(requestBody, BtctTurkHelper.DefaultJsonSerializerOptions);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        }

        if (HasCredentials)
        {
            var nonce = BtctTurkHelper.GenerateNonce();
            var signature = BtctTurkHelper.GenerateSignature(nonce, _apiKey!, _secretKey!);

            request.Headers.Add("X-PCK", _apiKey);
            request.Headers.Add("X-Signature", signature);
            request.Headers.Add("X-Stamp", nonce);
        }

        using var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(content, BtctTurkHelper.DefaultJsonSerializerOptions);
    }

    public Task<T?> GetAsync<T>(string endpoint)
       => SendRequestAsync<T>(HttpMethod.Get, endpoint);

    public Task<T?> PostAsync<T>(string endpoint, object requestBody)
        => SendRequestAsync<T>(HttpMethod.Post, endpoint, requestBody);

    public Task<T?> DeleteAsync<T>(string endpoint)
        => SendRequestAsync<T>(HttpMethod.Delete, endpoint);
}
