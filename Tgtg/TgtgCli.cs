using System;
using System.Threading;
using Hazebroek.Tgtg.Flow;
using Hazebroek.Tgtg.Infra;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg
{
    internal sealed class TgtgCli
    {
        private readonly ILogger<TgtgCli> _logger;

        public TgtgCli(ILogger<TgtgCli> logger)
        {
            _logger = logger;
        }

        public int Execute(IServiceProvider di, string[] args)
        {
            _logger.LogInformation("Application started");

            di.GetRequiredService<PrintBannerStep>().Execute();

            var app = new CommandLineApplication
            {
                Name = "Tgtg",
                Description = "Tools voor het platform To Good To Go."
            };

            app.HelpOption();
            var addUserOpt = app.Option(
                "-a |--add", "Voeg gebruiker toe", CommandOptionType.NoValue);
            var runOpt = app.Option(
                "-r |--run", "Start de applicatie", CommandOptionType.NoValue);
            var listUsersOpt = app.Option(
                "-l |--list", "Print gebruikers", CommandOptionType.NoValue);
            var removeUserOpt = app.Option(
                "--remove", "Verwijder een gebruiker", CommandOptionType.NoValue);
            var debugOpt = app.Option(
                "-d |--debug", "Toon debug informatie", CommandOptionType.NoValue);

            app.OnExecuteAsync(async cancelToken =>
            {
                var cancelSource = new CancellationTokenSource();
                Console.CancelKeyPress += (sender, eventArgs) => cancelSource.Cancel();

                if (addUserOpt.HasValue())
                    await di.GetRequiredService<AddNewUserStep>().Execute(cancelSource.Token);
                else if (listUsersOpt.HasValue())
                    di.GetRequiredService<PrintUsersStep>().Execute(cancelSource.Token);
                else if (removeUserOpt.HasValue())
                    di.GetRequiredService<RemoveUserStep>().Execute(cancelSource.Token);
                else if (runOpt.HasValue())
                    await di.GetRequiredService<LoopInitiatorStep>().Execute(di, cancelSource.Token);
                else if (debugOpt.HasValue())
                    di.GetRequiredService<PrintDebugStep>().Execute();
                else
                    app.ShowHelp();

                return 0;
            });

            return app.Execute(args);
        }
    }
}