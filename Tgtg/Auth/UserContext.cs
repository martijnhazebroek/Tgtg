using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Auth
{
    public sealed class UserContext
    {
        [JsonProperty("ifttt_tokens")] public ICollection<string> IftttTokens = new Collection<string>();
        [JsonProperty("user_id")] public long? UserId { get; set; }
        [JsonProperty("display_name")] public string? UserDisplayName { get; set; }
        [JsonProperty("email")] public string? Email { get; set; }
        [JsonProperty("access_token")] public string? AccessToken { get; set; }
        [JsonProperty("refresh_token")] public string? RefreshToken { get; set; }
        
        [JsonProperty("push_sent_items")] public ICollection<PushNotificationSent> PushNotificationsSent = new Collection<PushNotificationSent>();
        
        public bool IsNotificationSent(string id) => PushNotificationsSent.Any(item => item.ItemId == id);

        public void DidSentNotification(string itemId)
        {
            PushNotificationsSent.Add(new PushNotificationSent{ItemId = itemId});
        }
    }

    public sealed class PushNotificationSent
    {
        [JsonProperty("item_id")] 
        public string ItemId { get; set; }
        
        [JsonProperty("date_time")] 
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}