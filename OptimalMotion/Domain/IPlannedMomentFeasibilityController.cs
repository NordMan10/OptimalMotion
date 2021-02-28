using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IPlannedMomentFeasibilityController
    {
        bool IsFeasibleWithoutProcessing(IMoment appearanceMoment, IInterval parkingPreliminaryStartMotion, 
            IInterval preliminaryStartMaxWaiting, IMoment plannedMoment);
        bool IsFeasibleWithProcessing(IMoment appearanceMoment, IInterval parkingSpecPlatformMotion,
            IInterval processingMaxWaiting, IInterval processing, IInterval specPlatformPreliminaryStartMotion,
            IInterval preliminaryStartMaxWaiting, IMoment plannedMoment);
    }
}
