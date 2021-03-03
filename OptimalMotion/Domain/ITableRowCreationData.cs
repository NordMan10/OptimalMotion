using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITableRowCreationData
    {
        int AircraftId { get; }
        string AppearanceMoment { get; }
        string SpecPlatformLeaveMoment { get; }
        string PreliminaryStartArrivalMoment { get; }
        string PlannedMoment { get; }
        string EngineStartMoment { get; }
        string MinProcessingWaiting { get; }
        string SafeMergeWaiting { get; }
        string MinPSWaiting { get; }
        bool NeedProcessing { get; }
        bool IsPlannedMomentFeasible { get; }
    }
}
