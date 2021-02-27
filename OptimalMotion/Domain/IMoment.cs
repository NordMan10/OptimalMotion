using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public interface IMoment : IComparable
    {
        Moments Type { get; set; }

        IMoment GetMoment();
        void SetMoment(int moment);
    }
}
