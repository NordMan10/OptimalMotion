using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ISerialAccessZone
    {
        int Id { get; }

        Dictionary<IMoment, IMoment> OccupationIntervals { get; }
        //Tuple<int, int> GetNewLastInterval(IInterval interval);
    }
}
