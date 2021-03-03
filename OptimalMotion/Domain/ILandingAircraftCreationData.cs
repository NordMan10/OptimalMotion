

namespace OptimalMoving.Domain
{
    public interface ILandingAircraftCreationData
    {
        IAircraftId Id { get; }
        int RunwayId { get; }
        ILandingAircraftCreationMoments CreationMoments { get; } 
        ILandingAircraftCreationIntervals CreationIntervals { get; }
    }
}
