using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IAircraftGenerator
    {
        ITakingOffAircraft GetTakingOffAircraft(int creationTime, IRunway runway, ISpecPlatform specPlatform);
        ILandingAircraft GetLandingAircraft(int creationTime, int runwayIndex = 0);
    }
}
