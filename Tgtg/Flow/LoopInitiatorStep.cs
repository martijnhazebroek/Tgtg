using System;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopInitiatorStep
    {
        public static async Task Execute(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var usersContextRepo = serviceProvider.GetRequiredService<UsersContextRepository>();
            if (usersContextRepo.TryRestore(out var usersContext))
            {
                await serviceProvider.GetRequiredService<LoopUsersStep>()
                    .Execute(usersContext.UserIds, serviceProvider, cancellationToken);
            }
            else
            {
                var step = serviceProvider.GetService<LoopNewUserStep>();
                await step.Execute(cancellationToken);
            }
        }
    }
}