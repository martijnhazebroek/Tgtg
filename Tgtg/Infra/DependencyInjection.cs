using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Hazebroek.Tgtg.Auth;
using Hazebroek.Tgtg.Flow;
using Hazebroek.Tgtg.Notify;
using Hazebroek.Tgtg.Pickups;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Infra
{
    internal static class DependencyInjection
    {
        internal static ServiceProvider Init()
        {
            var serviceCollection = new ServiceCollection();

            RegisterUserContext(serviceCollection);
            RegisterHttpClients(serviceCollection);
            RegisterSteps(serviceCollection);
            RegisterDebuggers(serviceCollection);
            
            return serviceCollection.BuildServiceProvider();
        }

        private static void RegisterDebuggers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Debugger>();
        }

        private static void RegisterSteps(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<LoopInitiatorStep>()
                .AddTransient<LoopUsersStep>()
                .AddTransient<LoopKnownUserStep>()
                .AddTransient<LoopNewUserStep>()
                .AddTransient<AddNewUserStep>()
                .AddTransient<RemoveUserStep>()
                .AddTransient<AskEmailPasswordStep>()
                .AddTransient<AskIftttTokensStep>()
                .AddTransient<FetchFavoritesStep>()
                .AddTransient<FetchFavoritesStep>()
                .AddTransient<FetchReportNotifyLoopStep>()
                .AddTransient<LoginStep>()
                .AddTransient<NotifyUsersStep>()
                .AddTransient<TryAutoLoginStep>()
                .AddTransient<PrintUsersStep>();
        }

        private static void RegisterHttpClients(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<PickupClient>(client =>
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

            serviceCollection.AddHttpClient<TokenClient>(client =>
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

            serviceCollection.AddHttpClient<TgtgNotifier>(client =>
            {
                client.BaseAddress = new Uri("https://maker.ifttt.com/trigger/");
                client.DefaultRequestHeaders.Clear();
            });
        }

        private static void RegisterUserContext(ServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<UserContextRepository>()
                .AddScoped<UserContext>()
                .AddSingleton<UsersContextRepository>();
        }
    }
}