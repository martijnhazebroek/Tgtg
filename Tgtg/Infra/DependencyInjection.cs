using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Hazebroek.Tgtg.Auth;
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
            serviceCollection.AddScoped<TgtgClient>();
            serviceCollection.AddScoped<UserStateRepository>();
            serviceCollection.AddSingleton<LoginContext>();

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
                
            return serviceCollection.BuildServiceProvider();
        }
    }
}