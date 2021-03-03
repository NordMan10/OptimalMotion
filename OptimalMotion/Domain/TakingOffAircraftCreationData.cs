using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class TakingOffAircraftCreationData : ITakingOffAircraftCreationData
    {
        public TakingOffAircraftCreationData(IAircraftId id, ITakingOffAircraftCreationMoments creationMoments,
            ITakingOffAircraftCreationIntervals creationIntervals, IRunway runway,
            ISpecPlatform specPlatform, bool processingIsNeeded)
        {
            Id = id;
            CreationMoments = creationMoments;
            CreationIntervals = creationIntervals;
            Runway = runway;
            SpecPlatform = specPlatform;
            ProcessingIsNeeded = processingIsNeeded;
        }

        public IAircraftId Id { get; }
        public ITakingOffAircraftCreationMoments CreationMoments { get; }
        public ITakingOffAircraftCreationIntervals CreationIntervals { get; }
        public IRunway Runway { get; }
        public ISpecPlatform SpecPlatform { get; }
        public bool ProcessingIsNeeded { get; }
    }
}
