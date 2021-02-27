using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class Interval : IInterval
    {
        public Interval(IMoment startMoment, IMoment endMoment, Intervals type = Intervals.Ordinary)
        {
            Type = type;
            StartMoment = startMoment;
            EndMoment = endMoment;
        }

        public Intervals Type { get; set; }
        public IMoment StartMoment { get; }
        public IMoment EndMoment { get; }
        public int GetIntervalDuration()
        {
            throw new NotImplementedException();
        }
    }
}
