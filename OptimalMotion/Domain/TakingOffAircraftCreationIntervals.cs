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
            int executiveStartMotion, int takeOff, int maxPreliminaryStartWaiting, int maxProcessingWaiting,
            int maxSafeMerge)
        {
            Intervals = new Dictionary<Intervals, int>
            {
                {Enums.Intervals.ParkingPreliminaryStartMotion, parkingPreliminaryStartMotion},
                {Enums.Intervals.ParkingSpecPlatformMotion, parkingSpecPlatformMotion},
                {Enums.Intervals.ProcessingWaiting, 0},
                {Enums.Intervals.SafeMergeWaiting, 0},
                {Enums.Intervals.Processing, aircraftProcessing},
                {Enums.Intervals.SpecPlatformPreliminaryStartMotion, specPlatformPreliminaryStartMotion},
                {Enums.Intervals.PreliminaryStartWaiting, 0},
                {Enums.Intervals.ExecutiveStartMotion, executiveStartMotion},
                {Enums.Intervals.TakeOff, takeOff},
                {Enums.Intervals.MaxPreliminaryStartWaiting, maxPreliminaryStartWaiting},
                {Enums.Intervals.MaxProcessingWaiting, maxProcessingWaiting},
                {Enums.Intervals.MaxSafeMerge, maxSafeMerge}
            };
        }

        public Dictionary<Intervals, int> Intervals { get; }
    }
}
