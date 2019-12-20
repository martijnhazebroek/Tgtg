using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AskEmailPasswordStep
    {
        private readonly ConsolePrinter _console;
        public AskEmailPasswordStep(ConsolePrinter console)
        {
            _console = console;
        }

        public Credentials Execute(LoginAttempt? loginAttempt = null)
        {
            string? email = null;
            if (loginAttempt?.Email == null)
                email = Prompt.GetString("Email: ");
            else
                _console.WriteLine($"Email: {loginAttempt.Email}");

            var password = Prompt.GetPassword("Wachtwoord: ");

            _console.WriteLine();

            return new Credentials
            {
                Email = email,
                Password = password
            };
        }
    }
}