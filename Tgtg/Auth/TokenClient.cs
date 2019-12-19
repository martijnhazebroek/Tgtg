using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Auth
{
    internal sealed class TokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly UserContextRepository _userContextRepo;

        public TokenClient(HttpClient httpClient, UserContextRepository userContextRepo)
        {
            _httpClient = httpClient;
            _userContextRepo = userContextRepo;
        }

        public async Task<RefreshTokenResponse> Refresh()
        {
            UserContext userContext = _userContextRepo.CurrentContext;
            if (userContext.RefreshToken == null)
                throw new InvalidOperationException("RefreshToken should not be null");
            if (userContext.AccessToken == null)
                throw new InvalidOperationException("AccessToken should not be null");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/v1/token/refresh")
            {
                Content = JsonResult.FromObject(new RefreshTokenRequest
                {
                    RefreshToken = userContext.RefreshToken
                })
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userContext.AccessToken);

            using var response =
                await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                );

            var stream = await response.Content.ReadAsStreamAsync();
            return !response.IsSuccessStatusCode
                ? RefreshTokenResponse.Failed()
                : stream.ReadAndDeserializeFromJson<RefreshTokenResponse>();
        }

        public async Task<LoginResponse> Login(Credentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "index.php/api_tgtg/login")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", credentials.Email),
                    new KeyValuePair<string, string>("password", credentials.Password),
                    new KeyValuePair<string, string>("device_id", "7412b321fa48d0a9"),
                    new KeyValuePair<string, string>("device_token", "1a065962-a28b-4b25-9a8f-742f1c24ac46"),
                    new KeyValuePair<string, string>("device_type", "2")
                })
            };

            using var response =
                await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                );

            var stream = await response.Content.ReadAsStreamAsync();
            response.EnsureSuccessStatusCode();

            var result = stream.ReadAndDeserializeFromJson<LoginResponse>();

            _userContextRepo.CurrentContext.UserDisplayName = result.DisplayName;
            _userContextRepo.CurrentContext.Email = credentials.Email;
            _userContextRepo.CurrentContext.AccessToken = result.AccessToken;
            _userContextRepo.CurrentContext.RefreshToken = result.RefreshToken;
            _userContextRepo.CurrentContext.UserId = result.UserId;

            return result;
        }
    }
}