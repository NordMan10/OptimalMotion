

namespace OptimalMoving.Domain
{
    public interface ITakingOffAircraftCreationData
    {
        IAircraftId Id { get; }
        ITakingOffAircraftCreationMoments CreationMoments { get; }
        ITakingOffAircraftCreationIntervals CreationIntervals { get; } 
        IRunway Runway { get; }
        ISpecPlatform SpecPlatform { get; } 
        bool ProcessingIsNeeded { get; }
    }
}
