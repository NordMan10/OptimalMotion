using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class LandingAircraftCreationIntervals : ILandingAircraftCreationIntervals
    {
        public LandingAircraftCreationIntervals(int landing)
        {
            Intervals = new Dictionary<Intervals, int>
            {
                {Enums.Intervals.Landing, landing}
            };
        }

        public Dictionary<Intervals, int> Intervals { get; }
    }
}
