using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopNewUserStep
    {
        private readonly FetchReportNotifyLoopStep _fetchReportNotifyLoopStep;
        private readonly LoginStep _loginStep;

        public LoopNewUserStep(
            LoginStep loginStep,
            FetchReportNotifyLoopStep fetchReportNotifyLoopStep
        )
        {
            _loginStep = loginStep;
            _fetchReportNotifyLoopStep = fetchReportNotifyLoopStep;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var credentials = AskEmailPasswordStep.Execute();
            var loginAttempt = await _loginStep.Execute(credentials);

            PrintWelcomeUserStep.Execute(loginAttempt.UserDisplayName);

            await _fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}