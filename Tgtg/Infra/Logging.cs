using Serilog;

namespace Hazebroek.Tgtg.Infra
{
    internal static class Logging
    {
        internal static ILogger Init()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("../tgtg-{Date}.log",
                    fileSizeLimitBytes: 100000000,
                    retainedFileCountLimit: 2
                )
                .CreateLogger();

            return Log.Logger;
        }
    }
}