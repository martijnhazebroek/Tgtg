using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal class TgtgNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly UserContextRepository _userContextRepo;
        private readonly Logger<TgtgNotifier> _logger;
        private readonly Dictionary<string, Collection<string>> _notificationSent =
            new Dictionary<string, Collection<string>>();

        public TgtgNotifier(
            HttpClient httpClient,
            UserContextRepository userContextRepo,
            Logger<TgtgNotifier> logger
        )
        {
            _httpClient = httpClient;
            _userContextRepo = userContextRepo;
            _logger = logger;
        }

        public void Notify(string itemId, string store, int numberOfItems)
        {
            _userContextRepo.CurrentContext.IftttTokens
                .Where(token =>
                {
                    if (!_notificationSent.ContainsKey(token)) _notificationSent.Add(token, new Collection<string>());
                    return !_notificationSent[token].Contains(itemId);
                })
                .ToList()
                .ForEach(async token =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, $"tgtg/with/key/{token}")
                    {
                        Content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("value1", store),
                            new KeyValuePair<string, string>("value2", numberOfItems.ToString())
                        })
                    };

                    using var response =
                        await _httpClient.SendAsync(
                            request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                        );

                    var _ = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();

                    _logger.LogInformation($"Successfully sent push message for {_userContextRepo.CurrentContext.UserDisplayName}");

                    _notificationSent[token].Add(itemId);
                });
        }

        public void RegisterTokens(IEnumerable<string> tokens)
        {
            tokens.ToList().ForEach(_userContextRepo.CurrentContext.IftttTokens.Add);
            _userContextRepo.Persist();
        }
    }
}