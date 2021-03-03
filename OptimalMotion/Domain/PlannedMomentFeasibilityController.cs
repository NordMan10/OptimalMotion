using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class PlannedMomentFeasibilityController : IPlannedMomentFeasibilityController
    {
        protected PlannedMomentFeasibilityController() {}

        private static PlannedMomentFeasibilityController instance;
        private static object syncRoot = new object();

        IPlannedMomentFeasibilityController IPlannedMomentFeasibilityController.GetInstance()
        {
            return GetInstance();
        }

        public static IPlannedMomentFeasibilityController GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new PlannedMomentFeasibilityController();
                }
            }
            return instance;
        }

        public bool IsFeasibleWithoutProcessing(ITakingOffAircraft aircraft)
        {
            return aircraft.Moments[Moments.Appearance].Value +
                aircraft.Intervals[Intervals.ParkingPreliminaryStartMotion] +
                aircraft.Intervals[Intervals.MaxPreliminaryStartWaiting] <= 
                aircraft.Moments[Moments.PlannedPreliminaryStartArrival].Value;
        }

        public bool IsFeasibleWithProcessing(ITakingOffAircraft aircraft)
        {
            return aircraft.Moments[Moments.Appearance].Value +
                   aircraft.Intervals[Intervals.ParkingSpecPlatformMotion] +
                   aircraft.Intervals[Intervals.MaxProcessingWaiting] +
                   aircraft.Intervals[Intervals.Processing] +
                   aircraft.Intervals[Intervals.SpecPlatformPreliminaryStartMotion] +
                   aircraft.Intervals[Intervals.MaxPreliminaryStartWaiting] <= 
                   aircraft.Moments[Moments.PlannedPreliminaryStartArrival].Value;
        }
    }
}
