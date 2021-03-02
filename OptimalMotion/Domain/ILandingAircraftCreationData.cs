

namespace OptimalMoving.Domain
{
    public interface ILandingAircraftCreationData
    {
        IAircraftId Id { get; }
        int RunwayIndex { get; }
        ILandingAircraftCreationMoments CreationMoments { get; } 
        ILandingAircraftCreationIntervals CreationIntervals { get; }
    }
}
