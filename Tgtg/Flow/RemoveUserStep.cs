using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using Colorful;
using Hazebroek.Tgtg.Auth;
using McMaster.Extensions.CommandLineUtils;
using Console = Colorful.Console;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class RemoveUserStep
    {
        private readonly UsersContextRepository _usersContextRepository;

        public RemoveUserStep(
            UsersContextRepository usersContextRepository
        )
        {
            _usersContextRepository = usersContextRepository;
        }

        public void Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            Console.WriteLine("Gebruiker verwijderen.");
            var email = Prompt.GetString("Email: ");
            Console.WriteLine();

            UserContext user = _usersContextRepository
                .FetchUsers()
                .SingleOrDefault(u => u.Email == email);

            if (user?.UserId != null)
            {
                _usersContextRepository.Remove(user.UserId.Value);
                PrintUserRemoved(user.UserDisplayName);
            }
            else
            {
                PrintUserCouldNotBeRemoved(email);
            }
        }

        private static void PrintUserCouldNotBeRemoved(string email)
        {
            Console.WriteLineFormatted(
                "{0} kon niet worden verwijderd.",
                Color.Aqua,
                new Formatter(email, Color.Red)
            );
        }

        private static void PrintUserRemoved(string displayName)
        {
            Console.WriteLineFormatted(
                "{0} is verwijderd.",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}