﻿using System.Collections.Generic;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class LandingAircraft : ILandingAircraft
    {
        public LandingAircraft(ILandingAircraftCreationData creationData)
        {
            Id = creationData.Id;
            runwayId = creationData.RunwayId;
            Moments = creationData.CreationMoments.Moments;
            Intervals = creationData.CreationIntervals.Intervals;
        }

        private int runwayId;

        public IAircraftId Id { get; set; }
        public Dictionary<Moments, IMoment> Moments { get; }
        public Dictionary<Intervals, int> Intervals { get; }

        public int GetRunwayId()
        {
            return runwayId;
        }

        public IInterval GetRunwayOccupationInterval()
        {
            var endMoment = new Moment(Moments[Enums.Moments.Landing].Value + Intervals[Enums.Intervals.Landing]);

            return new Interval(Moments[Enums.Moments.Landing], endMoment);
        }
    }
}
