using System;

namespace Hazebroek.Tgtg.Infra
{
    internal static class TimeSpanExtensions
    {
        public static DateTime Ago(this TimeSpan source) => Ago(source, DateTime.Now);

        public static DateTime Ago(this TimeSpan source, DateTime now) => now.Subtract(source);
    }
}