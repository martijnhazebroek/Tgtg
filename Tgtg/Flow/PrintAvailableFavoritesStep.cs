using System;
using System.Drawing;
using System.Linq;
using Colorful;
using Hazebroek.Tgtg.Infra;
using Hazebroek.Tgtg.Pickups;

namespace Hazebroek.Tgtg.Flow
{
    internal class PrintAvailableFavoritesStep
    {
        private readonly ConsolePrinter _console;

        public PrintAvailableFavoritesStep(ConsolePrinter console)
        {
            _console = console;
        }

        public void Execute(AvailableFavoritesResponse favorites)
        {
            _console.WriteLine();
            _console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            favorites.StoreItems
                .ToList()
                .ForEach(row =>
                {
                    if (row.HasItems)
                        _console.WriteLineFormatted(
                            "{0}: heeft {1} item(s) beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter(row.ItemsAvailable, Color.LawnGreen)
                        );
                    else
                        _console.WriteLineFormatted(
                            "{0}: heeft {1} items beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter("geen", Color.Red)
                        );
                });
        }
    }
}