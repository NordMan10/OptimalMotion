using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class TableRowCreationData : ITableRowCreationData
    {
        public TableRowCreationData(int aircraftId, string appearanceMoment, string specPlatformLeaveMoment,  string preliminaryStartArrivalMoment, string plannedMoment, string engineStartMoment,
            string minProcessingWaiting, string safeMergeWaiting, string minPSWaiting, bool needProcessing, bool isPlannedMomentFeasible)
        {
            AircraftId = aircraftId;
            AppearanceMoment = appearanceMoment;
            SpecPlatformLeaveMoment = specPlatformLeaveMoment;
            PreliminaryStartArrivalMoment = preliminaryStartArrivalMoment;
            PlannedMoment = plannedMoment;
            EngineStartMoment = engineStartMoment;
            MinProcessingWaiting = minProcessingWaiting;
            MinPSWaiting = minPSWaiting;
            SafeMergeWaiting = safeMergeWaiting;
            NeedProcessing = needProcessing;
            IsPlannedMomentFeasible = isPlannedMomentFeasible;
        }

        public int AircraftId { get; }
        public string AppearanceMoment { get; }
        public string SpecPlatformLeaveMoment { get; }
        public string PreliminaryStartArrivalMoment { get; }
        public string PlannedMoment { get; }
        public string EngineStartMoment { get; }
        public string MinProcessingWaiting { get; }
        public string SafeMergeWaiting { get; }
        public string MinPSWaiting { get; }
        public bool NeedProcessing { get; }
        public bool IsPlannedMomentFeasible { get; }
    }
}
