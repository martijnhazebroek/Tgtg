using System;
using System.Threading;
using System.Threading.Tasks;
using Coravel.Invocable;
using Hazebroek.Tgtg.Flow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    public sealed class LoopInitiatorInvocable : ICancellableInvocable, IInvocable
    {
        private readonly ILogger<LoopInitiatorInvocable> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly LoopInitiatorStep _loop;

        public LoopInitiatorInvocable(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<LoopInitiatorInvocable>>();
            _loop = _serviceProvider.GetRequiredService<LoopInitiatorStep>();
        }

        public async Task Invoke()
        {
            _logger.LogInformation("IInvocable started at: {time}", DateTimeOffset.Now);

            await _loop.Execute(_serviceProvider, CancellationToken);

            _logger.LogInformation("IInvocable stopped at: {time}", DateTimeOffset.Now);
        }

        public CancellationToken CancellationToken { get; set; }
    }
}