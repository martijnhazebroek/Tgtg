using System.Linq;
using System.Threading;
using Colorful;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Flow
{
    internal class PrintUsersStep
    {
        private readonly ConsolePrinter _console;
        private readonly UsersContextRepository _usersContextRepo;

        public PrintUsersStep(
            ConsolePrinter console,
            UsersContextRepository usersContextRepo
        )
        {
            _console = console;
            _usersContextRepo = usersContextRepo;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            _usersContextRepo
                .FetchUsers()
                .ToList()
                .ForEach(uc =>
                    _console.WriteLine($"Gebruiker: {uc.UserDisplayName}, E-mail: {uc.Email}, ID: {uc.UserId}")
                );
        }
    }
}