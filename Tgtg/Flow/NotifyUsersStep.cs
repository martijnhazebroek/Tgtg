using System.Linq;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;
using MediatR;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class NotifyUsersStep
    {
        private readonly IMediator _mediator;
        private readonly UserContextRepository _userContextRepo;

        public NotifyUsersStep(IMediator mediator, UserContextRepository userContextRepo)
        {
            _mediator = mediator;
            _userContextRepo = userContextRepo;
        }

        public async Task Execute(AvailableFavoritesResponse favorites)
        {
            favorites.StoreItems
                .Where(si => si.HasItems)
                .Where(si =>
                    !_userContextRepo.CurrentContext!.IsNotificationSent(
                        si.Item!.Id!,
                        si.PurchaseEnd!
                    )
                )
                .ToList()
                .ForEach(async si =>
                {
                    _userContextRepo.CurrentContext.DidSentNotification(si.Item!.Id!, si.PurchaseEnd!);

                    await _mediator.Publish(new ItemNotification
                    (
                        _userContextRepo!.CurrentContext!.UserDisplayName!,
                        si.Item!.Id!,
                        si.Store!.Name!,
                        si.ItemsAvailable,
                        si.Item!.Picture!.Uri!
                    ));
                });

            await _userContextRepo.Persist();
        }
    }
}