using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class TakingOffAircraftCreationIntervals : ITakingOffAircraftCreationIntervals
    {
        public TakingOffAircraftCreationIntervals(int parkingPreliminaryStartMotion, int parkingSpecPlatformMotion,
            int aircraftProcessing, int specPlatformPreliminaryStartMotion,
            int executiveStartMotion, int takeOff)
        {
            Intervals = new Dictionary<Intervals, int>();
            Intervals.Add(Enums.Intervals.ParkingPreliminaryStartMotion, parkingPreliminaryStartMotion);
            Intervals.Add(Enums.Intervals.ParkingSpecPlatformMotion, parkingSpecPlatformMotion);
            Intervals.Add(Enums.Intervals.AircraftProcessingWaiting, 0);
            Intervals.Add(Enums.Intervals.SafeMergeWaiting, 0);
            Intervals.Add(Enums.Intervals.AircraftProcessing, aircraftProcessing);
            Intervals.Add(Enums.Intervals.SpecPlatformPreliminaryStartMotion, specPlatformPreliminaryStartMotion);
            Intervals.Add(Enums.Intervals.PreliminaryStartWaiting, 0);
            Intervals.Add(Enums.Intervals.ExecutiveStartMotion, executiveStartMotion);
            Intervals.Add(Enums.Intervals.TakeOff, takeOff);
        }

        public Dictionary<Intervals, int> Intervals { get; }
    }
}
