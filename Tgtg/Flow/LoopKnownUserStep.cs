using System;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;
using ExecutionContext = Hazebroek.Tgtg.Infra.ExecutionContext;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopKnownUserStep
    {
        private readonly PrintWelcomeUserStep _printWelcomeUserStep;
        private readonly AskEmailPasswordStep _askEmailPasswordStep;
        private readonly PrintUserCouldNotAutoLoginStep _printUserCouldNotAutoLoginStep;
        private readonly ExecutionContext _executionContext;

        public LoopKnownUserStep(
            PrintWelcomeUserStep printWelcomeUserStep,
            AskEmailPasswordStep askEmailPasswordStep,
            PrintUserCouldNotAutoLoginStep printUserCouldNotAutoLoginStep,
            ExecutionContext executionContext
        )
        {
            _printWelcomeUserStep = printWelcomeUserStep;
            _askEmailPasswordStep = askEmailPasswordStep;
            _printUserCouldNotAutoLoginStep = printUserCouldNotAutoLoginStep;
            _executionContext = executionContext;
        }

        public async Task Execute(long userId, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var autoLoginStep = scope.ServiceProvider.GetRequiredService<TryAutoLoginStep>();
            var loginStep = scope.ServiceProvider.GetRequiredService<LoginStep>();
            var fetchReportNotifyLoopStep = scope.ServiceProvider.GetRequiredService<FetchReportNotifyLoopStep>();

            var loginAttempt = await autoLoginStep.Execute(userId);
            if (loginAttempt.Status == LoginStatus.Reauthenticate)
            {
                _printUserCouldNotAutoLoginStep.Execute(loginAttempt);
                if (!_executionContext.HasPrompt) return;
                
                var credentials = _askEmailPasswordStep.Execute(loginAttempt);
                loginAttempt = await loginStep.Execute(credentials);
            }

            _printWelcomeUserStep.Execute(loginAttempt.UserDisplayName!);

            await fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}