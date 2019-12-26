using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class SlackNotification
    {
        public SlackNotification(string text)
        {
            Text = text;
        }
            
        [JsonProperty("text")] public string Text { get; }
    }
}