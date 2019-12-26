using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttNotifier
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IftttNotifier> _logger;

        private readonly Dictionary<string, Collection<string>> _notificationSent =
            new Dictionary<string, Collection<string>>();

        private readonly UserContextRepository _userContextRepo;

        public IftttNotifier(
            HttpClient httpClient,
            UserContextRepository userContextRepo,
            ILogger<IftttNotifier> logger
        )
        {
            _httpClient = httpClient;
            _userContextRepo = userContextRepo;
            _logger = logger;
        }

        public void Notify(ItemNotification notification)
        {
            _userContextRepo.CurrentContext.IftttTokens
                .Where(token =>
                {
                    if (!_notificationSent.ContainsKey(token)) _notificationSent.Add(token, new Collection<string>());
                    return !_notificationSent[token].Contains(notification.ItemId);
                })
                .ToList()
                .ForEach(async token =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, $"tgtg/with/key/{token}")
                    {
                        Content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("value1", notification.StoreName),
                            new KeyValuePair<string, string>("value2", notification.Quantity.ToString()),
                            new KeyValuePair<string, string>("value3", notification.StorePicture.AbsoluteUri),
                        })
                    };

                    using var response =
                        await _httpClient.SendAsync(
                            request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()
                        );

                    var _ = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();

                    _logger.LogInformation(
                        $"Successfully sent push message for {_userContextRepo.CurrentContext.UserDisplayName} regarding {notification.StoreName}");

                    _notificationSent[token].Add(notification.ItemId);
                });
        }

        public void RegisterTokens(IEnumerable<string> tokens)
        {
            tokens.ToList().ForEach(_userContextRepo.CurrentContext.IftttTokens.Add);
            _userContextRepo.Persist();
        }
    }
}