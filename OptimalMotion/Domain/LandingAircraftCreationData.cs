

namespace OptimalMoving.Domain
{
    public class LandingAircraftCreationData : ILandingAircraftCreationData
    {
        public LandingAircraftCreationData(IAircraftId id, ILandingAircraftCreationMoments creationMoments,
            ILandingAircraftCreationIntervals creationIntervals, int runwayIndex)
        {
            Id = id;
            RunwayId = runwayIndex;
            CreationMoments = creationMoments;
            CreationIntervals = creationIntervals;
        }

        public IAircraftId Id { get; }
        public int RunwayId { get; }
        public ILandingAircraftCreationMoments CreationMoments { get; }
        public ILandingAircraftCreationIntervals CreationIntervals { get; }
    }
}
