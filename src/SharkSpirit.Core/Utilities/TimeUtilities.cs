using System;
using System.Diagnostics;

namespace SharkSpirit.Core.Utilities
{
    public class TimeUtilities
    {
        public static TimeSpan ConvertRawToTimestamp(long delta)
        {
            return new TimeSpan(delta == 0 ? 0 : (delta * TimeSpan.TicksPerSecond) / Stopwatch.Frequency);
        }
    }
}