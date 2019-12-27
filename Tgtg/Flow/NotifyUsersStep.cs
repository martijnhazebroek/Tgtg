using System.Linq;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
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

        public void Execute(AvailableFavoritesResponse favorites)
        {
            favorites.StoreItems
                .Where(si => si.HasItems)
                .Where(si =>
                    !_userContextRepo.CurrentContext.IsNotificationSentPast(si.Item.Id, 1.Hours())
                )
                .ToList()
                .ForEach(async si =>
                {
                    await _mediator.Publish(new ItemNotification
                    (
                        _userContextRepo!.CurrentContext!.UserDisplayName!,
                        si.Item!.Id!,
                        si.Store!.Name!,
                        si.ItemsAvailable,
                        si.Item!.Picture!.Uri!
                    ));

                    _userContextRepo.CurrentContext.DidSentNotification(si.Item.Id);
                    await _userContextRepo.Persist();
                });
        }
    }
}