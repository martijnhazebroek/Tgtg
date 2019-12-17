using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class PickupClient
    {
        private readonly HttpClient _httpClient;
        private readonly LoginContext _loginContext;

        public PickupClient(HttpClient httpClient, LoginContext loginContext)
        {
            _httpClient = httpClient;
            _loginContext = loginContext;
        }

        public async Task<AvailableFavoritesResponse> FetchFavorites()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "item/v3/")
            {
                Content = JsonResult.FromObject(new AvailableFavoritesRequest
                {
                    UserId = _loginContext.UserId
                })
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loginContext.AccessToken);

            try
            {
                using var response =
                    await _httpClient.SendAsync(
                        request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                    );

                var stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                return stream.ReadAndDeserializeFromJson<AvailableFavoritesResponse>();
            }
            catch (IOException _)
            {
                return await Task.FromResult(new AvailableFavoritesResponse());
            }
        }
    }
}