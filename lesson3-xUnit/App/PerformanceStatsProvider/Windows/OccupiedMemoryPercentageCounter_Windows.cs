using System.Diagnostics;

namespace App.PerformanceStatsProvider.Windows
{
    internal class OccupiedMemoryPercentageCounter_Windows : IPerformanceCounter
    {
        private PerformanceCounter _counter;
        public OccupiedMemoryPercentageCounter_Windows()
        {
            _counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _counter.NextValue();
        }

        public float NextValue()
        {
            return _counter.NextValue();
        }
    }
}
