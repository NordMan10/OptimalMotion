using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class AircraftGenerator : IAircraftGenerator
    {
        public AircraftGenerator(IAircraftIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        private readonly IAircraftIdGenerator idGenerator;
        private readonly Random random = new Random();
        private const int maxProcessingWaitingTime = 180;
        private const int maxPreliminaryStartWaitingTime = 240;
        private const int safeMergeValue = 120;


        public ITakingOffAircraft GetTakingOffAircraft(int creationTime, IRunway runway, ISpecPlatform specPlatform)
        {
            var creationData = GetTakingOffAircraftCreationData(creationTime, runway, specPlatform);
            return new TakingOffAircraft(creationData);
        }

        public ILandingAircraft GetLandingAircraft(int creationTime, int runwayIndex = 0)
        {
            var creationData = GetLandingAircraftCreationData(creationTime, runwayIndex);
            return new LandingAircraft(creationData);
        }

        
        private ITakingOffAircraftCreationData GetTakingOffAircraftCreationData(int creationTime, IRunway runway, ISpecPlatform specPlatform)
        {
            var id = idGenerator.GetUniqueAircraftId();

            var probablyPlannedMoments = new List<int> {creationTime + 360, creationTime + 420};
            var creationMoments = new TakingOffAircraftCreationMoments(creationTime, 
                probablyPlannedMoments[random.Next(0, probablyPlannedMoments.Count)]);

            var creationIntervals = GetTakingOffAircraftCreationIntervals();

            var processingIsNeededVariants = new List<bool> {false, true};
            var processingIsNeeded = processingIsNeededVariants[random.Next(0, processingIsNeededVariants.Count)];

            return new TakingOffAircraftCreationData(id, creationMoments, creationIntervals, runway, specPlatform, maxProcessingWaitingTime,
                maxPreliminaryStartWaitingTime, safeMergeValue, processingIsNeeded);
        }

        private ITakingOffAircraftCreationIntervals GetTakingOffAircraftCreationIntervals()
        {
            var parkingPSMotions = new List<int> { 180, 210, 240 };
            var parkingSpecPlatformMotions = new List<int> { 30, 90, 70 };
            var processing = new List<int> { 30, 40, 50 };
            var specPlatformPSMotions = new List<int> { 30, 90, 70 };
            var executiveStartMotions = new List<int> { 20, 30, 40 };
            var takeOff = new List<int> { 60, 80, 90 };

            return new TakingOffAircraftCreationIntervals(
                parkingPSMotions[random.Next(0, parkingPSMotions.Count)],
                parkingSpecPlatformMotions[random.Next(0, parkingSpecPlatformMotions.Count)],
                processing[random.Next(0, processing.Count)],
                specPlatformPSMotions[random.Next(0, specPlatformPSMotions.Count)],
                executiveStartMotions[random.Next(0, executiveStartMotions.Count)],
                takeOff[random.Next(0, takeOff.Count)]);
        }

        private ILandingAircraftCreationData GetLandingAircraftCreationData(int creationTime, int runwayIndex)
        {
            var id = idGenerator.GetUniqueAircraftId();

            var creationMoments = new LandingAircraftCreationMoments(creationTime);

            var landingTime = new List<int> { 60, 75, 30 };
            var creationIntervals = new LandingAircraftCreationIntervals(landingTime[random.Next(landingTime.Count)]);

            return new LandingAircraftCreationData(id, creationMoments, creationIntervals, runwayIndex);
        }
    }
}
