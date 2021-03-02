using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITakingOffAircraftCreationData
    {
        IAircraftId Id { get; }
        ITakingOffAircraftCreationMoments CreationMoments { get; }
        ITakingOffAircraftCreationIntervals CreationIntervals { get; } 
        IRunway Runway { get; }
        ISpecPlatform SpecPlatform { get; } 
        int MaxProcessingWaitingTime { get; }
        int MaxPreliminaryStartWaitingTime { get; }
        int SafeMergeValue { get; } 
        bool ProcessingIsNeeded { get; }
    }
}
