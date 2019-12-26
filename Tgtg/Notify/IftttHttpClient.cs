using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttHttpClient
    {
        private readonly HttpClient _httpClient;

        public IftttHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            response.EnsureSuccessStatusCode();
        }
    }
}