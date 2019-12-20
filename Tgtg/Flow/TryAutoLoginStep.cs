using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class TryAutoLoginStep
    {
        private readonly TokenClient _tokenClient;
        private readonly UserContextRepository _userContextRepo;

        public TryAutoLoginStep(
            UserContextRepository userContextRepo,
            TokenClient tokenClient
        )
        {
            _userContextRepo = userContextRepo;
            _tokenClient = tokenClient;
        }

        public async Task<LoginAttempt> Execute(long userId)
        {
            var userStateRestored = _userContextRepo.TryRestore(userId, out var restored);
            if (!userStateRestored || restored == null) 
                return LoginAttempt.UserHasToAuthenticate(restored?.UserDisplayName!);

            var refresh = await _tokenClient.Refresh();
            if (!refresh.IsSuccess) return LoginAttempt.UserHasToAuthenticate(restored.UserDisplayName!);
            await _userContextRepo.Persist();
            
            return LoginAttempt.KnownUser(restored.Email!, restored.UserDisplayName!);
        }
    }
}