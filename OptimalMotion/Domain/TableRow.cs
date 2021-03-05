

using System.ComponentModel;

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

        [DisplayName("Id")]
        public int Id { get; }

        [DisplayName("Id ВС")]
        public int AircraftId { get; }
        
        [DisplayName("Т появ.")]
        public string AppearanceMoment { get; }

        [DisplayName("Т плщ/покид")]
        public string SpecPlatformLeaveMoment { get; }

        [DisplayName("Т приб/прст")]
        public string PreliminaryStartArrivalMoment { get; }

        [DisplayName("Т план/прст")]
        public string PlannedMoment { get; }

        [DisplayName("Т отпр")]
        public string EngineStartMoment { get; }

        [DisplayName("t ож/об")]
        public string MinProcessingWaiting { get; }

        [DisplayName("t ож/об/без")]
        public string SafeMergeWaiting { get; }

        [DisplayName("t ож/прст")]
        public string MinPSWaiting { get; }

        [DisplayName("Нужна ли обработка?")]
        public bool NeedProcessing { get; }

        [DisplayName("Плановый момент выполним?")]
        public bool IsPlannedMomentFeasible { get; }
    }
}
