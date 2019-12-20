using System.Drawing;
using Colorful;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class PrintUserCouldNotAutoLoginStep
    {
        private readonly ILogger<PrintUserCouldNotAutoLoginStep> _logger;
        public PrintUserCouldNotAutoLoginStep(ILogger<PrintUserCouldNotAutoLoginStep> logger)
        {
            _logger = logger;
        }

        public void Execute(LoginAttempt attempt)
        {
            _logger.LogWarning($"User {attempt.UserDisplayName} could not login with the given tokens.");
            
            Console.WriteLineFormatted(
                "Gebruiker {0} kon niet automatisch worden ingelogd en moet opnieuw worden toegevoegd.",
                Color.Aqua,
                new Formatter(attempt.UserDisplayName ?? "'onbekend'", Color.Red)
            );
            
            Console.WriteLine();
        }
    }
}