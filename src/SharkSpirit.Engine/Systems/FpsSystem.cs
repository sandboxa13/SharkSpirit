using System;
using SharkSpirit.Core;

namespace SharkSpirit.Engine.Systems
{
    public class FpsSystem : SystemBase
    {
        private double _timePerTick;
        private long _amountOfTicks;
        private double _nextTime;
        private int _updates;
        private DateTime _time;
        private int _lastFps;
        private float _lastMspf;
        public FpsSystem(long amountOfTicks, IContainer container)
        {
            _time = DateTime.Now;
            _amountOfTicks = amountOfTicks;
            _updates = 0;
            _timePerTick = (double)TimeSpan.TicksPerSecond / amountOfTicks;
            _nextTime = DateTime.Now.Ticks;
        }



        public void Tick()
        {
            if ((_nextTime - DateTime.Now.Ticks) <= 0)
            {
                _nextTime += _timePerTick;
                Tick();
                _updates++;
            }
            if ((DateTime.Now - _time).Ticks > 10000000)
            {
                _time += new TimeSpan(TimeSpan.TicksPerSecond);
                _lastFps = _updates;
                _lastMspf = 1000.0f / _updates;
                _updates = 0;
            }
        }

        public int GetFps()
        {
            return _lastFps;
        }

        public float GetMspf()
        {
            return _lastMspf;
        }
    }
}
