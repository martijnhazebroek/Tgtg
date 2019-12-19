using System.Threading.Tasks;
using Hazebroek.Tgtg.Pickups;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class FetchFavoritesStep
    {
        private readonly PickupClient _pickupClient;

        public FetchFavoritesStep(PickupClient pickupClient)
        {
            _pickupClient = pickupClient;
        }

        public async Task<AvailableFavoritesResponse> Execute()
        {
            return await _pickupClient.FetchFavorites();
        }
    }
}