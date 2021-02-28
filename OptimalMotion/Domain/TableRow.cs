using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class TableRow : ITableRow
    {
        public TableRow(int id, IAircraftId aircraftId, IMoment engineStartMoment, bool isPlannedMomentFeasible)
        {
            Id = id;
            AircraftId = aircraftId;
            EngineStartMoment = engineStartMoment;
            IsPlannedMomentFeasible = isPlannedMomentFeasible;
        }

        public int Id { get; }
        public IAircraftId AircraftId { get; }
        public IMoment EngineStartMoment { get; }
        public bool IsPlannedMomentFeasible { get; }
    }
}
