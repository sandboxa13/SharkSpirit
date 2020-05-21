using System;
using System.Diagnostics;
using SharkSpirit.Core.Utilities;

namespace SharkSpirit.Engine
{
    public class GameTimer
    {
        private readonly long _startRawTime;
        private long _lastRawTime;

        public GameTimer()
        {
            _startRawTime = 0;
            _lastRawTime = 0;
            Reset();
        }
        
        /// <summary>
        /// Gets the start time when this timer was created.
        /// </summary>
        public TimeSpan StartTime { get; private set; }

        /// <summary>
        /// Gets the total time elasped since the last reset or when this timer was created.
        /// </summary>
        public TimeSpan TotalTime { get; private set; }
        
        /// <summary>
        /// Gets the elapsed time since the previous call to <see cref="Tick"/>.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
        
        
        /// <summary>
        /// Resets this instance. <see cref="TotalTime"/> is set to zero.
        /// </summary>
        public void Reset()
        {
            Reset(TimeSpan.Zero);
        }

        /// <summary>
        /// Resets this instance. <see cref="TotalTime" /> is set to startTime.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        public void Reset(TimeSpan startTime)
        {
            StartTime = startTime;
            TotalTime = startTime;
        }
        
        public void Tick()
        {
            var rawTime = Stopwatch.GetTimestamp();
            TotalTime = StartTime + new TimeSpan((long)Math.Round((double) TimeUtilities.ConvertRawToTimestamp(rawTime - _startRawTime).Ticks));

            ElapsedTime = TimeUtilities.ConvertRawToTimestamp(rawTime - _lastRawTime);

            if (ElapsedTime < TimeSpan.Zero)
            {
                ElapsedTime = TimeSpan.Zero;
            }

            _lastRawTime = rawTime;
        }
    }
}