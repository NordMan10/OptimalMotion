

namespace OptimalMoving.Domain
{
    public class LandingAircraftCreationData : ILandingAircraftCreationData
    {
        public LandingAircraftCreationData(IAircraftId id, ILandingAircraftCreationMoments creationMoments,
            ILandingAircraftCreationIntervals creationIntervals, int runwayIndex)
        {
            Id = id;
            RunwayIndex = runwayIndex;
            CreationMoments = creationMoments;
            CreationIntervals = creationIntervals;
        }

        public IAircraftId Id { get; }
        public int RunwayIndex { get; }
        public ILandingAircraftCreationMoments CreationMoments { get; }
        public ILandingAircraftCreationIntervals CreationIntervals { get; }
    }
}
