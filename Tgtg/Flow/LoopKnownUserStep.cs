using System;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopKnownUserStep
    {
        private readonly PrintWelcomeUserStep _printWelcomeUserStep;
        private readonly AskEmailPasswordStep _askEmailPasswordStep;

        public LoopKnownUserStep(
            PrintWelcomeUserStep printWelcomeUserStep,
            AskEmailPasswordStep askEmailPasswordStep
        )
        {
            _printWelcomeUserStep = printWelcomeUserStep;
            _askEmailPasswordStep = askEmailPasswordStep;
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
                var credentials = _askEmailPasswordStep.Execute(loginAttempt);
                loginAttempt = await loginStep.Execute(credentials);
            }

            _printWelcomeUserStep.Execute(loginAttempt.UserDisplayName!);

            await fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}