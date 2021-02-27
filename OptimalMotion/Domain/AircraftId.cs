using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class AircraftId : IAircraftId
    {
        public AircraftId(int id)
        {
            this.id = id;
        }
        private readonly int id;

        public int GetId()
        {
            return id;
        }

        
    }
}
