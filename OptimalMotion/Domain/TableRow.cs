

namespace OptimalMoving.Domain
{
    public class TableRow : ITableRow
    {
        public TableRow(int id, ITableRowCreationData creationData)
        {
            Id = id;
            AircraftId = creationData.AircraftId;
            AppearanceMoment = creationData.AppearanceMoment;
            SpecPlatformLeaveMoment = creationData.SpecPlatformLeaveMoment;
            PreliminaryStartArrivalMoment = creationData.PreliminaryStartArrivalMoment;
            PlannedMoment = creationData.PlannedMoment;
            NeedProcessing = creationData.NeedProcessing;
            EngineStartMoment = creationData.EngineStartMoment;
            MinProcessingWaiting = creationData.MinProcessingWaiting;
            MinPSWaiting = creationData.MinPSWaiting;
            SafeMergeWaiting = creationData.SafeMergeWaiting;
            IsPlannedMomentFeasible = creationData.IsPlannedMomentFeasible;
        }

        public int Id { get; }
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
