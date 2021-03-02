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

        public bool IsFeasibleWithoutProcessing(ITakingOffAircraftCreationData creationData)
        {
            return creationData.CreationMoments.Moments[Moments.Appearance].Value +
                creationData.CreationIntervals.Intervals[Intervals.ParkingPreliminaryStartMotion] +
                creationData.MaxPreliminaryStartWaitingTime <= creationData.CreationMoments
                    .Moments[Moments.PlannedPreliminaryStartArrival].Value;
        }

        public bool IsFeasibleWithProcessing(ITakingOffAircraftCreationData creationData)
        {
            return creationData.CreationMoments.Moments[Moments.Appearance].Value +
                   creationData.CreationIntervals.Intervals[Intervals.ParkingSpecPlatformMotion] +
                   creationData.MaxProcessingWaitingTime +
                   creationData.CreationIntervals.Intervals[Intervals.Processing] +
                   creationData.CreationIntervals.Intervals[Intervals.SpecPlatformPreliminaryStartMotion] +
                   creationData.MaxPreliminaryStartWaitingTime + creationData.MaxPreliminaryStartWaitingTime <=
                   creationData.CreationMoments
                       .Moments[Moments.PlannedPreliminaryStartArrival].Value;
        }
    }
}
