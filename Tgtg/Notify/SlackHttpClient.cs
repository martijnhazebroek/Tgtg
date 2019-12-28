using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Infra;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class SlackHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SlackHttpClient> _logger;

        public SlackHttpClient(
            HttpClient httpClient,
            ILogger<SlackHttpClient> logger
        )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendNotification(SlackNotification notification)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"TRYTHFEAH/BRYE5PC8L/g4Ai0TD9VSAMC4RwMo6KFGis")
            {
                Content = JsonResult.FromObject(notification)
            };

            using var response =
                await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                );

            var _ = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Unsuccessful response: {response.StatusCode}");
            }
        }
    }
}