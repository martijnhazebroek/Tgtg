using System.Threading.Tasks;
using Hazebroek.Tgtg.Notify;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class NotifyAdminStep
    {
        private readonly SlackNotifier _notifier;

        public NotifyAdminStep(SlackNotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task Execute() => await _notifier.SendKeepAlive();
    }
}