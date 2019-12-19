using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    internal sealed class RefreshTokenRequest
    {
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
    }

    public class RefreshTokenResponse
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }

        public bool IsSuccess { get; private set; } = true;

        public static RefreshTokenResponse Failed()
        {
            return new RefreshTokenResponse {IsSuccess = false};
        }
    }
}