using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopNewUserStep
    {
        private readonly LoginStep _loginStep;
        private readonly FetchReportNotifyLoopStep _fetchReportNotifyLoopStep;
        private readonly PrintWelcomeUserStep _printWelcomeUserStep;

        public LoopNewUserStep(
            LoginStep loginStep,
            PrintWelcomeUserStep printWelcomeUserStep,
            FetchReportNotifyLoopStep fetchReportNotifyLoopStep
        )
        {
            _loginStep = loginStep;
            _printWelcomeUserStep = printWelcomeUserStep;
            _fetchReportNotifyLoopStep = fetchReportNotifyLoopStep;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var credentials = AskEmailPasswordStep.Execute();
            var loginAttempt = await _loginStep.Execute(credentials);

            _printWelcomeUserStep.Execute(loginAttempt.UserDisplayName);

            await _fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}