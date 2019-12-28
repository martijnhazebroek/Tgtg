using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IftttHttpClient> _logger;

        public IftttHttpClient(
            HttpClient httpClient, 
            ILogger<IftttHttpClient> logger
        )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task SendToWebHook(IftttRequest request, CancellationToken cancellationToken)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"tgtg/with/key/{request.Token}")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("value1", request.Value1),
                    new KeyValuePair<string, string>("value2", request.Value2),
                    new KeyValuePair<string, string>("value3", request.Value3),
                })
            };

            using var response =
                await _httpClient.SendAsync(
                    httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken
                );

            var _ = await response.Content.ReadAsStreamAsync();
            // response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Unsuccessful response: {response.StatusCode}");
            }
        }
    }
}