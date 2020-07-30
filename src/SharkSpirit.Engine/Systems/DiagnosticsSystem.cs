using System;
using System.Diagnostics;

namespace SharkSpirit.Engine.Systems
{
    public class DiagnosticsSystem : SystemBase
    {
        private readonly PerformanceCounter _processPerformanceCounter;
        private readonly PerformanceCounter _ramCounter;

        private float _prevM;
        private float _prevP;

        private TimeSpan _lastCheck;

        public DiagnosticsSystem()
        {
            _processPerformanceCounter =
                new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

            _ramCounter = new PerformanceCounter("Memory", "Available MBytes", true);
        }

        public SystemInformation CollectInformation(GameTimer timer)
        {
            // check if last "information collecting" was less than 1 second ago
            if (!(timer.TotalTime.TotalSeconds - _lastCheck.TotalSeconds > 1f))
                return new SystemInformation((float) Math.Round(_prevP / 10, 2), _prevM);
            
            // set last check to total
            _lastCheck = timer.TotalTime;

            
            // collect CPU usage info
            _prevP = _processPerformanceCounter.NextValue();

            // collect RAM usage info
            using (var proc = Process.GetCurrentProcess())
            {
                _prevM = proc.PrivateMemorySize64 / (1024 * 1024);
            }
            
            return new SystemInformation((float) Math.Round(_prevP / 10, 2), _prevM);
        }
    }

    public class SystemInformation
    {
        public SystemInformation(float cpuUsage, float memoryUsage)
        {
            CpuUsage = cpuUsage;
            MemoryUsage = memoryUsage;
        }

        public float CpuUsage { get; }
        public float MemoryUsage { get; }
    }
}