using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IPlannedMomentFeasibilityController
    {
        bool IsFeasibleWithoutProcessing(IMoment appearanceMoment, int parkingPreliminaryStartMotion, 
            int preliminaryStartMaxWaiting, IMoment plannedMoment);
        bool IsFeasibleWithProcessing(IMoment appearanceMoment, int parkingSpecPlatformMotion,
            int processingMaxWaiting, int processing, int specPlatformPreliminaryStartMotion,
            int preliminaryStartMaxWaiting, IMoment plannedMoment);
    }
}
