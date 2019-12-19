using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    public sealed class UserContext
    {
        [JsonProperty("ifttt_tokens")] public Collection<string> IftttTokens = new Collection<string>();
        [JsonProperty("user_id")] public long? UserId { get; set; }
        [JsonProperty("display_name")] public string UserDisplayName { get; set; }
        [JsonProperty("email")] public string Email { get; set; }
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
    }
}