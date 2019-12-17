using System;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using Hazebroek.Tgtg.Pickups;

namespace Hazebroek.Tgtg
{
    internal sealed class TgtgClient
    {
        private readonly PickupClient _pickupClient;
        private readonly TokenClient _tokenClient;
        private readonly UserStateRepository _userStateRepo;

        public TgtgClient(
            TokenClient tokenClient,
            PickupClient pickupClient,
            UserStateRepository userStateRepo 
        )
        {
            _tokenClient = tokenClient ?? throw new ArgumentNullException(nameof(tokenClient));
            _pickupClient = pickupClient ?? throw new ArgumentNullException(nameof(pickupClient));
            _userStateRepo = userStateRepo ?? throw new ArgumentNullException(nameof(userStateRepo));
        }

        public async Task<LoginStatus> Init()
        {
            bool userStateRestored = _userStateRepo.TryRestoreLoginContext();
            if (!userStateRestored)
            {
                return LoginStatus.Reauthenticate;
            }
            RefreshTokenResponse refresh = await _tokenClient.Refresh();
            if (!refresh.IsSuccess) return LoginStatus.Reauthenticate;
            await _userStateRepo.SafeLoginContext();
            return LoginStatus.Success;
        }

        public async Task<string> Login(string username, string password)
        {
            var result = await _tokenClient.Login(username, password);
            await _userStateRepo.SafeLoginContext();
            return result.DisplayName;
        }

        public Task<AvailableFavoritesResponse> FetchFavorites() => 
            _pickupClient.FetchFavorites();
    }
}