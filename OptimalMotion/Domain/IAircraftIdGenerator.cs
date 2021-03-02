using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface IAircraftIdGenerator
    {
        IAircraftId GetUniqueAircraftId();
        IAircraftIdGenerator GetInstance(int initIdValue);
    }
}
