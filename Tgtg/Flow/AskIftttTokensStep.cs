using Hazebroek.Tgtg.Notify;
using McMaster.Extensions.CommandLineUtils;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class AskIftttTokensStep
    {
        private readonly IftttRepository _iftttRepo;

        public AskIftttTokensStep(IftttRepository iftttRepo)
        {
            _iftttRepo = iftttRepo;
        }

        public void Execute()
        {
            var tokens = Prompt.GetString("IFTTT tokens (gescheiden door comma): ");
            if (tokens != null)
            {
                _iftttRepo.RegisterTokens(tokens.Split(","));
            }
        }
    }
}