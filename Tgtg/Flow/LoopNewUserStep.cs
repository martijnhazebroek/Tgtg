using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class LoopNewUserStep
    {
        private readonly FetchReportNotifyLoopStep _fetchReportNotifyLoopStep;
        private readonly AskEmailPasswordStep _askEmailPasswordStep;
        private readonly LoginStep _loginStep;
        private readonly PrintWelcomeUserStep _printWelcomeUserStep;

        public LoopNewUserStep(
            LoginStep loginStep,
            PrintWelcomeUserStep printWelcomeUserStep,
            FetchReportNotifyLoopStep fetchReportNotifyLoopStep,
            AskEmailPasswordStep askEmailPasswordStep
        )
        {
            _loginStep = loginStep;
            _printWelcomeUserStep = printWelcomeUserStep;
            _fetchReportNotifyLoopStep = fetchReportNotifyLoopStep;
            _askEmailPasswordStep = askEmailPasswordStep;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var credentials = _askEmailPasswordStep.Execute();
            var loginAttempt = await _loginStep.Execute(credentials);

            // TODO: assumes login succeeded
            _printWelcomeUserStep.Execute(loginAttempt.UserDisplayName!);

            await _fetchReportNotifyLoopStep.Execute(cancellationToken);
        }
    }
}