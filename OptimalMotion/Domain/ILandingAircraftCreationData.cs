using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public interface ILandingAircraftCreationData
    {
        IAircraftId Id { get; }
        ILandingAircraftCreationMoments CreationMoments { get; } 
        ILandingAircraftCreationIntervals CreationIntervals { get; }
    }
}
