using System.Drawing;
using Colorful;
using Hazebroek.Tgtg.Auth;

namespace Hazebroek.Tgtg.Flow
{
    internal static class PrintUserCouldNotAutologin
    {
        public static void Execute(LoginAttempt attempt)
        {
            Console.WriteLineFormatted(
                "Gebruiker {0} kon niet automatisch worden ingelogd en moet opnieuw worden toegevoegd.",
                Color.Aqua,
                new Formatter(attempt.UserDisplayName ?? "'onbekend'", Color.Red)
            );
            
            Console.WriteLine();
        }
    }
}