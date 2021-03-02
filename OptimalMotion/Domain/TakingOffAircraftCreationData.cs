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
            ISpecPlatform specPlatform, int maxProcessingWaitingTime,
            int maxPreliminaryStartWaitingTime, int safeMergeValue, bool processingIsNeeded)
        {
            
        }

        public IAircraftId Id { get; }
        public ITakingOffAircraftCreationMoments CreationMoments { get; }
        public ITakingOffAircraftCreationIntervals CreationIntervals { get; }
        public IRunway Runway { get; }
        public ISpecPlatform SpecPlatform { get; }
        public int MaxProcessingWaitingTime { get; }
        public int MaxPreliminaryStartWaitingTime { get; }
        public int SafeMergeValue { get; }
        public bool ProcessingIsNeeded { get; }
    }
}
