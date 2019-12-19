using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Colorful;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AddNewUserStep
    {
        private readonly AskEmailPasswordStep _askEmailPasswordStep;
        private readonly AskIftttTokensStep _askIftttTokensStep;
        private readonly LoginStep _loginStep;

        public AddNewUserStep(
            AskEmailPasswordStep askEmailPasswordStep,
            LoginStep loginStep,
            AskIftttTokensStep askIftttTokensStep
        )
        {
            _askEmailPasswordStep = askEmailPasswordStep;
            _loginStep = loginStep;
            _askIftttTokensStep = askIftttTokensStep;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var credentials = _askEmailPasswordStep.Execute();
            var loginAttempt = await _loginStep.Execute(credentials);
            
            _askIftttTokensStep.Execute();

            PrintUserAdded(loginAttempt.UserDisplayName);
        }

        private static void PrintUserAdded(string displayName)
        {
            Console.WriteLineFormatted(
                "{0} is toegevoegd.",
                Color.Aqua,
                new Formatter(displayName, Color.LawnGreen)
            );
        }
    }
}