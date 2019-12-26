using System.Collections.Generic;
using System.Linq;
using Hazebroek.Tgtg.Auth;
using Microsoft.Extensions.Logging;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class IftttRepository
    {
        private readonly ILogger<IftttRepository> _logger;
        private readonly UserContextRepository _userContextRepo;

        public IftttRepository(
            UserContextRepository userContextRepo,
            ILogger<IftttRepository> logger
        )
        {
            _userContextRepo = userContextRepo;
            _logger = logger;
        }

        public void RegisterTokens(IEnumerable<string> tokens)
        {
            _logger.LogInformation("Added ifttt tokens.");
            tokens.ToList().ForEach(_userContextRepo.CurrentContext.IftttTokens.Add);
            _userContextRepo.Persist();
        }
    }
}