using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoginStep
    {
        private readonly TokenClient _tokenClient;
        private readonly UserContextRepository _userContextRepo;
        private readonly UsersContextRepository _usersContextRepository;

        public LoginStep(
            UserContextRepository userContextRepo,
            UsersContextRepository usersContextRepository,
            TokenClient tokenClient
        )
        {
            _userContextRepo = userContextRepo;
            _usersContextRepository = usersContextRepository;
            _tokenClient = tokenClient;
        }

        public async Task<LoginAttempt> Execute(Credentials credentials)
        {
            var response = await _tokenClient.Login(credentials);
            // TODO fix failed login scenario
            _usersContextRepository.AddUser(response.UserId);
            await _userContextRepo.Persist();
            return LoginAttempt.KnownUser(credentials.Email!, response.DisplayName!);
        }
    }
}