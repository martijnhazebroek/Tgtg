using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Colorful;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Infra;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;
using McMaster.Extensions.CommandLineUtils;
using Console = Colorful.Console;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var di = DependencyInjection.Init();

            var client = di.GetService<TgtgClient>();
            var notifier = di.GetService<TgtgNotifier>();

            var user = await Login(client, notifier);
            WelcomeUser(user);

            while (true)
            {
                var favorites = await client.FetchFavorites();
                Report(favorites);
                Notify(favorites, notifier);

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        private static void WelcomeUser(string user)
        {
            Console.WriteLine("Welkom {0}!\n", user, Color.Aqua);
        }

        private static void Report(AvailableFavoritesResponse favorites)
        {
            Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));

            favorites.StoreItems
                .ToList()
                .ForEach(row =>
                {
                    if (row.HasItems)
                    {
                        Console.WriteLineFormatted(
                            "{0}: heeft {1} item(s) beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter(row.ItemsAvailable, Color.LawnGreen)
                        );
                    }
                    else
                    {
                        Console.WriteLineFormatted(
                            "{0}: heeft {1} items beschikbaar.",
                            Color.MediumPurple,
                            new Formatter(row.Store.Name, Color.Aqua),
                            new Formatter("geen", Color.Red)
                        );
                    }
                });
            Console.WriteLine();
        }

        private static void Notify(AvailableFavoritesResponse favorites, TgtgNotifier notifier)
        {
            favorites.StoreItems
                .Where(si => si.HasItems)
                .ToList()
                .ForEach(si => { notifier.Notify(si.Item.Id, si.Store.Name, si.ItemsAvailable); });
        }

        private static async Task<string> Login(TgtgClient client, TgtgNotifier notifier)
        {
            var loginStatus = await client.Init();
            if (loginStatus == LoginStatus.Reauthenticate)
            {
                var username = Prompt.GetString("Gebruikersnaam: ");
                var password = Prompt.GetPassword("Wachtwoord: ");
                var displayName = await client.Login(username, password);

                var iftttTokens = Prompt.GetString("IFTTT tokens (gescheiden door comma): ");
                notifier.RegisterTokens(iftttTokens.Split(","));

                return displayName;
            }

            return "terug";
        }
    }
}