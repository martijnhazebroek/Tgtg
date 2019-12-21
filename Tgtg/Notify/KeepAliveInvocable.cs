using System.Threading.Tasks;
using Coravel.Invocable;
using Hazebroek.Tgtg.Flow;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class KeepAliveInvocable : IInvocable
    {
        private readonly NotifyAdminStep _notifyAdminStep;

        public KeepAliveInvocable(NotifyAdminStep notifyAdminStep)
        {
            _notifyAdminStep = notifyAdminStep;
        }

        public async Task Invoke()
        {
            await _notifyAdminStep.Execute();
        }
    }
}