using Hazebroek.Tgtg.Notify;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AskIftttTokensStep
    {
        private readonly IftttNotifier _notifier;

        public AskIftttTokensStep(IftttNotifier notifier)
        {
            _notifier = notifier;
        }

        public void Execute()
        {
            var tokens = Prompt.GetString("IFTTT tokens (gescheiden door comma): ");
            if (tokens != null)
            {
                _notifier.RegisterTokens(tokens.Split(","));
            }
        }
    }
}