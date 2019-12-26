using System.Threading.Tasks;

namespace Hazebroek.Tgtg.Notify
{
    internal sealed class SlackKeepAliveNotifier
    {
        private readonly SlackHttpClient _slack;

        public SlackKeepAliveNotifier(SlackHttpClient slack)
        {
            _slack = slack;
        }

        public async Task SendKeepAlive()
        {
            await _slack.SendNotification(new SlackNotification("Alive and kicking!"));
        }
    }
}