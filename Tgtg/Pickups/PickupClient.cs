using System;
using System.IO;
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
        private readonly UserContextRepository _userContextRepo;
        private readonly ILogger<PickupClient> _logger;

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
                response.EnsureSuccessStatusCode();

                _logger.LogInformation($"Successfully fetched favorites for {userContext.UserDisplayName}");

                return stream.ReadAndDeserializeFromJson<AvailableFavoritesResponse>();
            }
            catch (IOException)
            {
                _logger.LogInformation($"Error while fetching favorites for {userContext.UserDisplayName}");

                return await Task.FromResult(new AvailableFavoritesResponse());
            }
        }
    }
}