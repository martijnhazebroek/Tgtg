using System;

namespace Hazebroek.Tgtg.Infra
{
    internal static class IntegerExtensions
    {
        public static TimeSpan Hours(this int source) => new TimeSpan(0, source, 0, 0);
    }
}