using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    public sealed class LoginResponse
    {
        [JsonProperty("display_name")] public string? DisplayName { get; set; }
        [JsonProperty("user_id")] public int UserId { get; set; }
        [JsonProperty("user_token")] public string? UserToken { get; set; }
        [JsonProperty("access_token")] public string? AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string? RefreshToken { get; set; }
    }
}