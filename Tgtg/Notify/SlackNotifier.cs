using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Infra;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class SlackNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SlackNotifier> _logger;

        public SlackNotifier(
            HttpClient httpClient,
            ILogger<SlackNotifier> logger
        )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendKeepAlive()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"TRYTHFEAH/BRYE5PC8L/g4Ai0TD9VSAMC4RwMo6KFGis")
            {
                Content = JsonResult.FromObject(new Payload("Alive and kicking!"))
            };

            using var response =
                await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                );

            var _ = await response.Content.ReadAsStreamAsync();
            response.EnsureSuccessStatusCode();
        }

        private class Payload
        {
            public Payload(string text)
            {
                Text = text;
            }
            
            [JsonProperty("text")] public string Text { get; }
        }

    }
}