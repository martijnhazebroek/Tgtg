using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class SlackItemNotifier : INotificationHandler<ItemNotification>
    {
        private readonly SlackHttpClient _slack;

        public SlackItemNotifier(SlackHttpClient slack)
        {
            _slack = slack;
        }

        public async Task Handle(ItemNotification notification, CancellationToken cancellationToken)
        {
            await _slack.SendNotification(
                new SlackNotification($"Sent push message to {notification.UserDisplayName} regarding {notification.StoreName}"));
        }
    }
}