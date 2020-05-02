using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Coravel;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Flow;
using Hazebroek.Tgtg.Infra;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg
{
    internal class Program
    {
        private static ServiceProvider? _serviceProvider;

        private static int Main(string[] args)
        {
            ExecutionContext executionContext = args.Contains("--worker")
                ? (ExecutionContext) new WorkerExecutionContext()
                : new CliExecutionContext();

            var builder = CreateHostBuilder(args, executionContext);
            if (executionContext.HasPrompt)
            {
                builder.RunConsoleAsync();
                var cli = _serviceProvider.GetRequiredService<TgtgCli>();
                return cli.Execute(_serviceProvider!, args);
            }

            var host = builder.Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler
                    .Schedule<KeepAliveInvocable>()
                    .Hourly();
                scheduler
                    .Schedule<LoopInitiatorInvocable>()
                    .HourlyAt(0);
                scheduler
                    .Schedule<LoopInitiatorInvocable>()
                    .HourlyAt(15);
                scheduler
                    .Schedule<LoopInitiatorInvocable>()
                    .HourlyAt(30);
                scheduler
                    .Schedule<LoopInitiatorInvocable>()
                    .HourlyAt(45);
            }).OnError(ex =>
            {
                var logger = host.Services.GetService<ILogger<Program>>();
                logger.LogError(ex, ex.Message);
            });
            host.Run();

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args, ExecutionContext executionContext)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureLogging(logging =>
                {
                    if (executionContext.HasPrompt) logging.ClearProviders();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    if (executionContext.HasPrompt)
                    {
                        services.AddTransient<ConsolePrinter, CliConsolePrinter>();
                        services.AddSingleton(executionContext);
                    }
                    else
                    {
                        services.AddTransient<ConsolePrinter, WorkerConsolePrinter>();
                        services.AddSingleton(executionContext);
                        services.AddScheduler();
                    }

                    services
                        .AddTransient<TgtgCli>()
                        .AddTransient<LoopInitiatorStep>()
                        .AddTransient<LoopUsersStep>()
                        .AddTransient<LoopNewUserStep>()
                        .AddTransient<LoopKnownUserStep>()
                        .AddTransient<AskEmailPasswordStep>()
                        .AddTransient<AddNewUserStep>()
                        .AddTransient<RemoveUserStep>()
                        .AddTransient<AskIftttTokensStep>()
                        .AddTransient<FetchFavoritesStep>()
                        .AddTransient<FetchReportNotifyLoopStep>()
                        .AddTransient<LoginStep>()
                        .AddTransient<NotifyUsersStep>()
                        .AddTransient<NotifyAdminStep>()
                        .AddTransient<TryAutoLoginStep>()
                        .AddTransient<PrintBannerStep>()
                        .AddTransient<PrintUsersStep>()
                        .AddTransient<PrintAvailableFavoritesStep>()
                        .AddTransient<PrintUserCouldNotAutoLoginStep>()
                        .AddTransient<PrintDebugStep>()
                        .AddTransient<PrintWelcomeUserStep>()
                        .AddTransient<KeepAliveInvocable>()
                        .AddTransient<LoopInitiatorInvocable>()
                        .AddTransient<IftttRepository>()
                        .AddTransient<SlackKeepAliveNotifier>()
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
                        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                        {
                            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(15),
                            PooledConnectionLifetime = TimeSpan.FromSeconds(60),
                            AutomaticDecompression = DecompressionMethods.GZip
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
                        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                        {
                            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(15),
                            PooledConnectionLifetime = TimeSpan.FromSeconds(60),
                            AutomaticDecompression = DecompressionMethods.GZip
                        });

                    services.AddHttpClient<IftttHttpClient>(client =>
                    {
                        client.BaseAddress = new Uri("https://maker.ifttt.com/trigger/");
                        client.DefaultRequestHeaders.Clear();
                    });

                    services.AddHttpClient<SlackHttpClient>(client =>
                    {
                        client.BaseAddress = new Uri("https://hooks.slack.com/services/");
                        client.DefaultRequestHeaders.Clear();
                    });

                    services.AddMediatR(Assembly.GetExecutingAssembly());

                    if (!executionContext.HasPrompt)
                        services.AddHostedService(factory =>
                            new Worker(services.BuildServiceProvider())
                        );
                    else
                        _serviceProvider = services.BuildServiceProvider();
                });
        }
    }
}