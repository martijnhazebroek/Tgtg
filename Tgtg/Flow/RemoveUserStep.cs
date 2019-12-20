using System.Drawing;
using System.Linq;
using System.Threading;
using Colorful;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class RemoveUserStep
    {
        private readonly UsersContextRepository _usersContextRepository;
        private readonly ConsolePrinter _console;

        public RemoveUserStep(
            UsersContextRepository usersContextRepository,
            ConsolePrinter console
        )
        {
            _usersContextRepository = usersContextRepository;
            _console = console;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            _console.WriteLine("Gebruiker verwijderen.");
            var email = Prompt.GetString("Email: ");
            _console.WriteLine();

            if (email == null) return;
            
            var user = _usersContextRepository
                .FetchUsers()
                .SingleOrDefault(u => u.Email! == email);

            if (user?.UserId != null)
            {
                _usersContextRepository.Remove(user.UserId.Value);
                PrintUserRemoved(user.UserDisplayName!);
            }
            else
            {
                PrintUserCouldNotBeRemoved(email);
            }
        }

        private void PrintUserCouldNotBeRemoved(string email)
        {
            _console.WriteLineFormatted(
                "{0} kon niet worden verwijderd.",
                Color.Aqua,
                new Formatter(email, Color.Red)
            );
        }

        private void PrintUserRemoved(string displayName)
        {
            _console.WriteLineFormatted(
                "{0} is verwijderd.",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}