using System.Drawing;
using Colorful;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Flow
{
    internal class PrintWelcomeUserStep
    {
        private readonly ConsolePrinter _console;

        public PrintWelcomeUserStep(ConsolePrinter console)
        {
            _console = console;
        }

        public void Execute(string displayName)
        {
            _console.WriteLineFormatted(
                "Welkom {0}!",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}