using System;
using System.Threading;
using System.Threading.Tasks;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopKnownUserStep
    {
        public static async Task Execute(long userId, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var autoLoginStep = scope.ServiceProvider.GetRequiredService<TryAutoLoginStep>();
            var loginStep = scope.ServiceProvider.GetRequiredService<LoginStep>();
            var fetchReportNotifyLoopStep = scope.ServiceProvider.GetRequiredService<FetchReportNotifyLoopStep>();

            var loginAttempt = await autoLoginStep.Execute(userId);
            if (loginAttempt.Status == LoginStatus.Reauthenticate)
            {
                var credentials = AskEmailPasswordStep.Execute(loginAttempt);
                loginAttempt = await loginStep.Execute(credentials);
            }

            PrintWelcomeUserStep.Execute(loginAttempt.UserDisplayName);

            await fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}