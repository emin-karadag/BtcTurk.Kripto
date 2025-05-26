using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BtcTurk.Kripto.Core.Helpers
{
    internal static class BtctTurkHelper
    {
        public static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
        };

        internal static string GenerateNonce()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        internal static string GenerateSignature(string nonce, string apiKey, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(apiKey + nonce);
            var secretBytes = Convert.FromBase64String(secretKey!);

            using var hmac = new HMACSHA256(secretBytes);
            var signatureBytes = hmac.ComputeHash(keyBytes);
            return Convert.ToBase64String(signatureBytes);
        }
    }
}
