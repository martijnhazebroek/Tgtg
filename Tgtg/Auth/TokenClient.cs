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
        private readonly LoginContext _loginContext;

        public TokenClient(HttpClient httpClient, LoginContext loginContext)
        {
            _httpClient = httpClient;
            _loginContext = loginContext;
        }

        public async Task<RefreshTokenResponse> Refresh()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/v1/token/refresh")
            {
                Content = JsonResult.FromObject(new RefreshTokenRequest
                {
                    RefreshToken = _loginContext.RefreshToken
                })
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _loginContext.AccessToken);

            using var response =
                await _httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                );

            var stream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                return RefreshTokenResponse.Failed();
            }

            return stream.ReadAndDeserializeFromJson<RefreshTokenResponse>();
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "index.php/api_tgtg/login")
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
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
            _loginContext.AccessToken = result.AccessToken;
            _loginContext.RefreshToken = result.RefreshToken;
            _loginContext.UserId = result.UserId;

            return result;
        }
    }
}