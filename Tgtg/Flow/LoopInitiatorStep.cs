using System;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopInitiatorStep
    {
        private readonly ILogger<LoopInitiatorStep> _logger;
        public LoopInitiatorStep(ILogger<LoopInitiatorStep> logger)
        {
            _logger = logger;
        }
        
        public async Task Execute(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var usersContextRepo = serviceProvider.GetRequiredService<UsersContextRepository>();
            if (usersContextRepo.TryRestore(out var usersContext))
            {
                await serviceProvider.GetRequiredService<LoopUsersStep>()
                    .Execute(usersContext.UserIds, serviceProvider, cancellationToken);
            }
            else
            {
                _logger.LogWarning("Unable to restore usersContext");
                var step = serviceProvider.GetService<LoopNewUserStep>();
                await step.Execute(cancellationToken);
            }
        }
    }
}