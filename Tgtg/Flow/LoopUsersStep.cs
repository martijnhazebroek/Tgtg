using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Flow
{
    public class LoopUsersStep
    {
        private readonly List<Task> _tasks;

        public LoopUsersStep()
        {
            _tasks = new List<Task>();
        }
        
        public async Task Execute(IEnumerable<long> userIds, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            foreach (var userId in userIds)
            {
                var tryAutoLoginStep = serviceProvider.GetService<TryAutoLoginStep>();
                var attempt = await tryAutoLoginStep.Execute(userId);
                if (attempt.Status == LoginStatus.Reauthenticate)
                {
                    PrintUserCouldNotAutologin.Execute(attempt);
                }
                else
                {
                    var loopKnownUserStep = serviceProvider.GetService<LoopKnownUserStep>();
                    _tasks.Add(loopKnownUserStep.Execute(userId, serviceProvider, cancellationToken));
                }
            }
            await Task.WhenAll(_tasks);
        }
    }
}