using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITableRowCreationData
    {
        IAircraftId AircraftId { get; }
        IMoment EngineStartMoment { get; }
        bool IsPlannedMomentFeasible { get; }
    }
}
