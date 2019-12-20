using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Pickups
{
    internal sealed class PickupClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PickupClient> _logger;
        private readonly UserContextRepository _userContextRepo;

        public PickupClient(
            HttpClient httpClient,
            UserContextRepository userContextRepo,
            ILogger<PickupClient> logger
        )
        {
            _httpClient = httpClient;
            _userContextRepo = userContextRepo;
            _logger = logger;
        }

        public async Task<AvailableFavoritesResponse> FetchFavorites()
        {
            var userContext = _userContextRepo.CurrentContext;
            if (!userContext.UserId.HasValue)
                throw new InvalidOperationException("UserId should not be null");
            if (userContext.AccessToken == null)
                throw new InvalidOperationException("AccessToken should not be null");

            var request = new HttpRequestMessage(HttpMethod.Post, "item/v3/")
            {
                Content = JsonResult.FromObject(new AvailableFavoritesRequest
                {
                    UserId = userContext.UserId.Value
                })
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);

            try
            {
                using var response =
                    await _httpClient.SendAsync(
                        request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                    );

                var stream = await response.Content.ReadAsStreamAsync();
                _logger.LogInformation($"Fetched favorites: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                return stream.ReadAndDeserializeFromJson<AvailableFavoritesResponse>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error while fetching favorites for {userContext.UserDisplayName}");

                return await Task.FromResult(new AvailableFavoritesResponse());
            }
        }
    }
}