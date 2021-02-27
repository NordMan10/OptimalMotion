using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public interface IInterval
    {
        Intervals Type { get; set; }
        IMoment StartMoment { get; }
        IMoment EndMoment { get; }

        int GetIntervalDuration();

    }
}
