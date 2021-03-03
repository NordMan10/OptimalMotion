

namespace OptimalMoving.Domain
{
    public interface IPlannedMomentFeasibilityController
    {
        IPlannedMomentFeasibilityController GetInstance();
        bool IsFeasibleWithoutProcessing(ITakingOffAircraft aircraft);
        bool IsFeasibleWithProcessing(ITakingOffAircraft aircraft);
    }
}
