using System;

namespace NPerf.Cons.WPFTest.Types
{
    public interface IDateRange
    {
        TimeSpan Duration { get; }

        DateTime EndTime { get; }

        DateTime StartTime { get; }

        bool Intersect(DateTime dateTime);
    }
}
