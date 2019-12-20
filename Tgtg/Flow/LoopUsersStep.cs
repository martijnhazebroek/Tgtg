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
        private readonly LoopKnownUserStep _loopKnownUserStep;
        private readonly List<Task> _tasks;

        public LoopUsersStep(
            LoopKnownUserStep loopKnownUserStep
        )
        {
            _tasks = new List<Task>();
            _loopKnownUserStep = loopKnownUserStep;
        }

        public async Task Execute(
            IEnumerable<long> userIds,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken
        )
        {
            foreach (var userId in userIds)
            {
                _tasks.Add(_loopKnownUserStep.Execute(userId, serviceProvider, cancellationToken));
            }

            await Task.WhenAll(_tasks);
        }
    }
}