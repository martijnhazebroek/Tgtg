using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Colorful;
using Hazebroek.Tgtg.Infra;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AddNewUserStep
    {
        private readonly AskIftttTokensStep _askIftttTokensStep;
        private readonly AskEmailPasswordStep _askEmailPasswordStep;
        private readonly ConsolePrinter _console;
        private readonly LoginStep _loginStep;

        public AddNewUserStep(
            LoginStep loginStep,
            AskIftttTokensStep askIftttTokensStep,
            AskEmailPasswordStep askEmailPasswordStep,
            ConsolePrinter console
        )
        {
            _loginStep = loginStep;
            _askIftttTokensStep = askIftttTokensStep;
            _askEmailPasswordStep = askEmailPasswordStep;
            _console = console;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var credentials = _askEmailPasswordStep.Execute();
            var loginAttempt = await _loginStep.Execute(credentials);

            _askIftttTokensStep.Execute();

            PrintUserAdded(loginAttempt!.UserDisplayName!);
        }

        private void PrintUserAdded(string displayName)
        {
            _console.WriteLineFormatted(
                "{0} is toegevoegd.",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}