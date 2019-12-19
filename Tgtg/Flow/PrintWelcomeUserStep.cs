using System.Drawing;
using Colorful;

namespace Hazebroek.Tgtg.Flow
{
    internal static class PrintWelcomeUserStep
    {
        public static void Execute(string displayName)
        {
            Console.WriteLineFormatted(
                "Welkom {0}!",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}