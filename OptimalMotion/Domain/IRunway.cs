﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IRunway : ISerialAccessZone
    {
        int GetPreliminaryStartMinDelayTime(IMoment startMoment, IMoment endMoment);
    }
}
