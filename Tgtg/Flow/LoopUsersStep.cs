using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Flow
{
    internal class LoopUsersStep
    {
        private readonly List<Task> _tasks;
        private readonly PrintUserCouldNotAutoLoginStep _printUserCouldNotAutoLoginStep;
        private readonly LoopKnownUserStep _loopKnownUserStep;

        public LoopUsersStep(
            PrintUserCouldNotAutoLoginStep printUserCouldNotAutoLoginStep,
            LoopKnownUserStep loopKnownUserStep
        )
        {
            _tasks = new List<Task>();
            _printUserCouldNotAutoLoginStep = printUserCouldNotAutoLoginStep;
            _loopKnownUserStep = loopKnownUserStep;
        }

        public async Task Execute(IEnumerable<long> userIds, IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            foreach (var userId in userIds)
            {
                var tryAutoLoginStep = serviceProvider.GetService<TryAutoLoginStep>();
                var attempt = await tryAutoLoginStep.Execute(userId);
                if (attempt.Status == LoginStatus.Reauthenticate)
                {
                    _printUserCouldNotAutoLoginStep.Execute(attempt);
                }
                else
                {
                    _tasks.Add(_loopKnownUserStep.Execute(userId, serviceProvider, cancellationToken));
                }
            }

            await Task.WhenAll(_tasks);
        }
    }
}