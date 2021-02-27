using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public interface IAircraft
    {
        IAircraftId Id { get; set; }

        Dictionary<Moments, IMoment> Moments { get; }
        Dictionary<Intervals, IInterval> Intervals { get; }

        IInterval GetRunwayOccupationInterval();
    }
}
