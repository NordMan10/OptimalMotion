using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ISpecPlatform : ISerialAccessZone
    {
        int Id { get; }

        Tuple<int, int> GetProcessingAndSafeMergeDelay(IInterval aircraftInterval, int safeMergeValueParam);
    }
}
