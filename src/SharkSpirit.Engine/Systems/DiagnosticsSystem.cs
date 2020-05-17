using System;
using System.Diagnostics;

namespace SharkSpirit.Engine.Systems
{
    public class DiagnosticsSystem : SystemBase
    {
        private readonly PerformanceCounter _processPerformanceCounter;
        private int _seconds;
        private float _prevM;
        private float _prevP;
        public DiagnosticsSystem()
        {
            _processPerformanceCounter =
                new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

        }

        public SystemInformation CollectInformation()
        {
            if (DateTime.Now.Second % 5 != 0)
            {
                return new SystemInformation((float) Math.Round(_prevP / 10, 2), _prevM);
            }
            
            var p = _processPerformanceCounter.NextValue();

            _prevP = p;
            _prevM = GC.GetTotalMemory(true) / 100000;
            
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