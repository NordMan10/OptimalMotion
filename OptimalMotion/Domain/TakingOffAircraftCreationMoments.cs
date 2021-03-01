﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class TakingOffAircraftCreationMoments : ITakingOffAircraftCreationMoments
    {
        public TakingOffAircraftCreationMoments(int appearance, int plannedPreliminaryStartArrival)
        {
            Moments = new Dictionary<Moments, IMoment>
            {
                {Enums.Moments.Appearance, new Moment(appearance)},
                {Enums.Moments.PlannedPreliminaryStartArrival, new Moment(plannedPreliminaryStartArrival)},
                {Enums.Moments.Departure, new Moment(0)},
                {Enums.Moments.PreliminaryStartArrival, new Moment(0)},
                {Enums.Moments.ExecutiveStartArrival, new Moment(0)},
                {Enums.Moments.TakeOff, new Moment(0)}
            };
        }

        public Dictionary<Moments, IMoment> Moments { get; }
    }
}
