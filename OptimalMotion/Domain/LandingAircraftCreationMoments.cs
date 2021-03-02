using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class LandingAircraftCreationMoments : ITakingOffAircraftCreationMoments
    {
        public LandingAircraftCreationMoments(int landing)
        {
            Moments = new Dictionary<Moments, IMoment>
            {
                {Enums.Moments.Landing, new Moment(landing)}
            };
        }

        public Dictionary<Moments, IMoment> Moments { get; }
    }
}
