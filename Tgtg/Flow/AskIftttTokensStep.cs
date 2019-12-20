using Hazebroek.Tgtg.Notify;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AskIftttTokensStep
    {
        private readonly TgtgNotifier _notifier;

        public AskIftttTokensStep(TgtgNotifier notifier)
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