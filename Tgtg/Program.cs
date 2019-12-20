using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Flow;
using Hazebroek.Tgtg.Infra;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        private static int Main(string[] args)
        {
            bool isWorker = args.Contains("--worker");

            var builder = CreateHostBuilder(args, isWorker);
            if (isWorker)
            {
                builder.Build().Run();
            }
            else
            {
                builder.RunConsoleAsync();
                var cli = _serviceProvider.GetRequiredService<TgtgCli>();
                return cli.Execute(_serviceProvider, args);
            }

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args, bool isWorker) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureLogging(logging =>
                {
                    if (!isWorker) logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    if (isWorker)
                    {
                        services.AddTransient<ConsolePrinter, WorkerConsolePrinter>();
                    }
                    else
                    {
                        services.AddTransient<ConsolePrinter, CliConsolePrinter>();
                    }
                    
                    services
                        .AddTransient<TgtgCli>()
                        .AddTransient<LoopInitiatorStep>()
                        .AddTransient<LoopUsersStep>()
                        .AddTransient<LoopNewUserStep>()
                        .AddTransient<LoopKnownUserStep>()
                        .AddTransient<AddNewUserStep>()
                        .AddTransient<RemoveUserStep>()
                        .AddTransient<AskIftttTokensStep>()
                        .AddTransient<FetchFavoritesStep>()
                        .AddTransient<FetchReportNotifyLoopStep>()
                        .AddTransient<LoginStep>()
                        .AddTransient<NotifyUsersStep>()
                        .AddTransient<TryAutoLoginStep>()
                        .AddTransient<PrintBannerStep>()
                        .AddTransient<PrintUsersStep>()
                        .AddTransient<PrintAvailableFavoritesStep>()
                        .AddTransient<PrintUserCouldNotAutoLoginStep>()
                        .AddTransient<PrintDebugStep>()
                        .AddTransient<PrintWelcomeUserStep>()
                        .AddScoped<UserContextRepository>()
                        .AddScoped<UserContext>()
                        .AddScoped<UsersContextRepository>();
                    
                    services.AddHttpClient<PickupClient>(client =>
                        {
                            client.BaseAddress = new Uri("https://apptoogoodtogo.com/api/");
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.AcceptLanguage.Add(
                                new StringWithQualityHeaderValue("nl-NL")
                            );
                            client.DefaultRequestHeaders.UserAgent.Add(
                                ProductInfoHeaderValue.Parse("TGTG/19.10.4")
                            );
                            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        })
                        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                        {
                            var handler = new HttpClientHandler
                            {
                                AutomaticDecompression = DecompressionMethods.GZip,
                                UseProxy = true
                            };
                            return handler;
                        });

                    services.AddHttpClient<TokenClient>(client =>
                        {
                            client.BaseAddress = new Uri("https://apptoogoodtogo.com/");
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.AcceptLanguage.Add(
                                new StringWithQualityHeaderValue("nl-NL")
                            );
                            client.DefaultRequestHeaders.UserAgent.Add(
                                ProductInfoHeaderValue.Parse("TGTG/19.10.4")
                            );
                            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                        })
                        .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                        {
                            var handler = new HttpClientHandler
                            {
                                AutomaticDecompression = DecompressionMethods.GZip,
                                UseProxy = true
                            };
                            return handler;
                        });

                    services.AddHttpClient<TgtgNotifier>(client =>
                    {
                        client.BaseAddress = new Uri("https://maker.ifttt.com/trigger/");
                        client.DefaultRequestHeaders.Clear();
                    });

                    if (isWorker)
                    {
                        services.AddHostedService(factory =>
                            new Worker(services.BuildServiceProvider())
                        );
                    }
                    else
                    {
                        _serviceProvider = services.BuildServiceProvider();
                    }
                });
    }
}