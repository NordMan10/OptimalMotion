using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Enums
{
    public enum Intervals
    {
        Ordinary,
        ParkingPreliminaryStartMotion,
        ParkingSpecPlatformMotion,
        AircraftProcessingWaiting,
        SafeMergeWaiting,
        Processing,
        SpecPlatformPreliminaryStartMotion,
        PreliminaryStartWaiting,
        ExecutiveStartMotion,
        TakeOff,
        Landing
    }
}
