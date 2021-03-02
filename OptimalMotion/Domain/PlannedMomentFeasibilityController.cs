using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class PlannedMomentFeasibilityController : IPlannedMomentFeasibilityController
    {
        public bool IsFeasibleWithoutProcessing(IMoment appearanceMoment, int parkingPreliminaryStartMotion,
            int preliminaryStartMaxWaiting, IMoment plannedMoment)
        {
            throw new NotImplementedException();
        }

        public bool IsFeasibleWithProcessing(IMoment appearanceMoment, int parkingSpecPlatformMotion, int processingMaxWaiting,
            int processing, int specPlatformPreliminaryStartMotion, int preliminaryStartMaxWaiting, IMoment plannedMoment)
        {
            throw new NotImplementedException();
        }
    }
}
