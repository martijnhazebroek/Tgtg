using System;
using System.Drawing;
using System.Linq;
using Colorful;
using Hazebroek.Tgtg.Pickups;
using Console = Colorful.Console;

namespace Hazebroek.Tgtg.Flow
{
    internal static class PrintAvailableFavoritesStep
    {
        public static void Execute(AvailableFavoritesResponse favorites)
        {
            Console.WriteLine();
            Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            favorites.StoreItems
                .ToList()
                .ForEach(row =>
                {
                    if (row.HasItems)
                        Console.WriteLineFormatted(
                            "{0}: heeft {1} item(s) beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter(row.ItemsAvailable, Color.LawnGreen)
                        );
                    else
                        Console.WriteLineFormatted(
                            "{0}: heeft {1} items beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter("geen", Color.Red)
                        );
                });
        }
    }
}