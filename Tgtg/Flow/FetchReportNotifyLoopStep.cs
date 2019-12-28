using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class FetchReportNotifyLoopStep
    {
        private readonly FetchFavoritesStep _favoritesStep;
        private readonly NotifyUsersStep _notifyUsersStep;
        private readonly PrintAvailableFavoritesStep _printAvailableFavoritesStep;

        public FetchReportNotifyLoopStep(
            PrintAvailableFavoritesStep printAvailableFavoritesStep,
            FetchFavoritesStep favoritesStep,
            NotifyUsersStep notifyUsersStep
        )
        {
            _printAvailableFavoritesStep = printAvailableFavoritesStep;
            _favoritesStep = favoritesStep;
            _notifyUsersStep = notifyUsersStep;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var favorites = await _favoritesStep.Execute();
                    _printAvailableFavoritesStep.Execute(favorites);
                    await _notifyUsersStep.Execute(favorites);

                    // Wait 2 minutes to start the next round.
                    await Task.Delay(TimeSpan.FromMinutes(2), cancellationToken);
                }
            }, cancellationToken);
        }
    }
}