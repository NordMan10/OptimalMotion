using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class LandingAircraft : ILandingAircraft
    {
        public LandingAircraft(IAircraftId id, ILandingAircraftCreationMoments creationMoments, 
            ILandingAircraftCreationIntervals creationIntervals)
        {
            Id = id;
            Moments = creationMoments.Moments;
            Intervals = creationIntervals.Intervals;
        }

        public IAircraftId Id { get; set; }
        public Dictionary<Moments, IMoment> Moments { get; }
        public Dictionary<Intervals, int> Intervals { get; }
        public IInterval GetRunwayOccupationInterval()
        {
            var endMoment = new Moment(Moments[Enums.Moments.Landing].Value + Intervals[Enums.Intervals.Landing]);

            return new Interval(Moments[Enums.Moments.Landing], endMoment);
        }
    }
}
