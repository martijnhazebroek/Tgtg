using System.Linq;
using System.Threading;
using Colorful;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Flow
{
    internal class PrintUsersStep
    {
        private readonly UsersContextRepository _usersContextRepo;

        public PrintUsersStep(UsersContextRepository usersContextRepo)
        {
            _usersContextRepo = usersContextRepo;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            _usersContextRepo
                .FetchUsers()
                .ToList()
                .ForEach(uc =>
                    Console.WriteLine($"Gebruiker: {uc.UserDisplayName}, E-mail: {uc.Email}, ID: {uc.UserId}")
                );
        }
    }
}