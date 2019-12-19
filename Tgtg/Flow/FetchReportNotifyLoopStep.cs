using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Flow
{
    internal sealed class FetchReportNotifyLoopStep
    {
        private readonly FetchFavoritesStep _favoritesStep;
        private readonly NotifyUsersStep _notifyUsersStep;

        public FetchReportNotifyLoopStep(
            FetchFavoritesStep favoritesStep,
            NotifyUsersStep notifyUsersStep
        )
        {
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
                    PrintAvailableFavoritesStep.Execute(favorites);
                    _notifyUsersStep.Execute(favorites);
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }
            }, cancellationToken);
        }
    }
}