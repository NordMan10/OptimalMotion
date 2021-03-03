using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITakingOffAircraft : IAircraft
    {
        bool ProcessingIsNeeded { get; }

        int GetSpecPlatformId();
        IMoment GetEngineStartMoment();
        IInterval GetSpecPlatformOccupationInterval();
    }
}
