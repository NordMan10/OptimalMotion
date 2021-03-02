

namespace OptimalMoving.Domain
{
    public interface IPlannedMomentFeasibilityController
    {
        IPlannedMomentFeasibilityController GetInstance();
        bool IsFeasibleWithoutProcessing(ITakingOffAircraftCreationData creationData);
        bool IsFeasibleWithProcessing(ITakingOffAircraftCreationData creationData);
    }
}
