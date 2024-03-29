using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttItemNotifier : INotificationHandler<ItemNotification>
    {
        private readonly IftttHttpClient _ifttt;
        private readonly ILogger<IftttItemNotifier> _logger;
        private readonly UserContextRepository _userContextRepo;

        public IftttItemNotifier(
            IftttHttpClient ifttt,
            UserContextRepository userContextRepo,
            ILogger<IftttItemNotifier> logger
        )
        {
            _ifttt = ifttt;
            _userContextRepo = userContextRepo;
            _logger = logger;
        }

        public async Task Handle(ItemNotification notification, CancellationToken cancellationToken)
        {
            var taskCollection = new List<Task>();
            _userContextRepo.CurrentContext.IftttTokens
                .ToList()
                .ForEach(async token =>
                {
                    taskCollection.Add(SendNotification(notification, token, cancellationToken));
                });

            await Task.WhenAll(taskCollection);
        }

        private async Task SendNotification(
            ItemNotification notification,
            string iftttToken,
            CancellationToken cancellationToken
        )
        {
            await _ifttt.SendToWebHook(
                new IftttRequest(iftttToken,
                    notification.StoreName,
                    notification.Quantity.ToString(),
                    notification.StorePicture.AbsoluteUri
                ),
                cancellationToken
            );

            _logger.LogInformation(
                $"Successfully sent push message for {_userContextRepo.CurrentContext.UserDisplayName} regarding {notification.StoreName}");
        }
    }
}