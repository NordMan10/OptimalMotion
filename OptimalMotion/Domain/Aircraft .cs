using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class Aircraft : IAircraft
    {
        public Aircraft()
        {
            
        }
        public IAircraftId Id { get; set; }
    }
}
