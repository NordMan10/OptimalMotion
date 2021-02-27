using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ISerialAccessZone
    {
        Dictionary<IMoment, IMoment> OccupationIntervals { get; }

        Tuple<Interval, Interval> GetLeftAndRightIntervalsRelative(IInterval interval);
        bool DoesIntervalIntersect(IInterval interval);
        void AddInterval(IInterval interval);
        void RemoveInterval(IInterval interval);
    }
}
