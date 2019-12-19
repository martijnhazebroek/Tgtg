using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    public sealed class UsersContext
    {
        [JsonProperty("user_ids")] public ISet<long> UserIds { get; set; } = new HashSet<long>();
    }
}