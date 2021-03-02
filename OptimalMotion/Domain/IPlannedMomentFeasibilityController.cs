using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IPlannedMomentFeasibilityController
    {
        IPlannedMomentFeasibilityController GetInstance();
        bool IsFeasibleWithoutProcessing(ITakingOffAircraftCreationData creationData);
        bool IsFeasibleWithProcessing(ITakingOffAircraftCreationData creationData);
    }
}
