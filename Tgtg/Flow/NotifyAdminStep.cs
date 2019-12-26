using System.Threading.Tasks;
using Hazebroek.Tgtg.Notify;
using MediatR;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class NotifyAdminStep
    {
        private readonly SlackKeepAliveNotifier _keepAliveNotifier;

        public NotifyAdminStep(SlackKeepAliveNotifier keepAliveNotifier)
        {
            _keepAliveNotifier = keepAliveNotifier;
        }

        public async Task Execute() => await _keepAliveNotifier.SendKeepAlive();
    }
}