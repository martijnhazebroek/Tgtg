using System.Linq;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class NotifyUsersStep
    {
        private readonly IftttNotifier _notifier;

        public NotifyUsersStep(IftttNotifier notifier)
        {
            _notifier = notifier;
        }

        public void Execute(AvailableFavoritesResponse favorites)
        {
            favorites.StoreItems
                .Where(si => si.HasItems)
                .ToList()
                .ForEach(si =>
                {
                    _notifier.Notify(si.Item!.Id!, si.Store!.Name!, si.ItemsAvailable);
                });
        }
    }
}