using System.Drawing;
using Colorful;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class PrintUserCouldNotAutoLoginStep
    {
        private readonly ConsolePrinter _console;
        private readonly ILogger<PrintUserCouldNotAutoLoginStep> _logger;

        public PrintUserCouldNotAutoLoginStep(
            ConsolePrinter console,
            ILogger<PrintUserCouldNotAutoLoginStep> logger
        )
        {
            _console = console;
            _logger = logger;
        }

        public void Execute(LoginAttempt attempt)
        {
            _logger.LogWarning($"User {attempt.UserDisplayName} could not login with the given tokens.");
            _console.WriteLineFormatted(
                "Gebruiker {0} kon niet automatisch worden ingelogd en moet opnieuw worden toegevoegd.",
                Color.Aqua,
                new Formatter(attempt.UserDisplayName ?? "'onbekend'", Color.Red)
            );
            _console.WriteLine();
        }
    }
}