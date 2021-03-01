using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class TakingOffAircraft : IAircraft
    {
        public TakingOffAircraft(IAircraftId id, ITakingOffAircraftCreationMoments creationMoments, 
            ITakingOffAircraftCreationIntervals creationIntervals, IRunway runway, 
            ISpecPlatform specPlatform, int maxProcessingWaitingTime,
            int maxPreliminaryStartWaitingTime, int safeMergeValue, bool processingIsNeeded)
        {
            Id = id;
            Moments = creationMoments.Moments;
            Intervals = creationIntervals.Intervals;
            this.runway = runway;
            this.specPlatform = specPlatform;
            this.maxProcessingWaitingTime = maxProcessingWaitingTime;
            this.maxPreliminaryStartWaitingTime = maxPreliminaryStartWaitingTime;
            this.safeMergeValue = safeMergeValue;
            ProcessingIsNeeded = processingIsNeeded;
        }

        private IRunway runway;
        private ISpecPlatform specPlatform;
        private int maxProcessingWaitingTime;
        private int maxPreliminaryStartWaitingTime;
        private int safeMergeValue;


        public IAircraftId Id { get; set; }
        public Dictionary<Moments, IMoment> Moments { get; }
        public Dictionary<Intervals, int> Intervals { get; }
        public bool ProcessingIsNeeded { get; }
        
        

        public IInterval GetRunwayOccupationInterval()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расчет интервала ожидания на ПРСТ: (1.1)
        /// </summary>
        /// <returns></returns>
        private int GetPreliminaryStartDelay()
        {
            var preliminaryStartDelay = runway.GetPreliminaryStartMinDelayTime(new Moment(GetExecutiveStartArrivalMoment()),
                new Moment(GetTakeOffMoment()));
            Intervals[Enums.Intervals.PreliminaryStartWaiting] = preliminaryStartDelay;
            return Intervals[Enums.Intervals.PreliminaryStartWaiting];
        }

        /// <summary>
        /// Расчет суммы интервала ожидания обработки и интервала для безопасного слияния: (1.2)
        /// </summary>
        /// <returns></returns>
        private int GetProcessingAndSafeMergeDelay()
        {
            var aircraftInterval = new Interval(new Moment(GetSpecPlatformArrivalMoment()),
                new Moment(GetSpecPlatformLeaveMoment()));
            var processingAndSafeMergeDelay =
                specPlatform.GetProcessingAndSafeMergeDelay(aircraftInterval, safeMergeValue);

            Intervals[Enums.Intervals.AircraftProcessingWaiting] = processingAndSafeMergeDelay.Item1;
            Intervals[Enums.Intervals.SafeMergeWaiting] = processingAndSafeMergeDelay.Item2;

            var sumOfDelays = Intervals[Enums.Intervals.AircraftProcessingWaiting] +
                              Intervals[Enums.Intervals.SafeMergeWaiting];

            return sumOfDelays;
        }

        /// <summary>
        /// Расчет момента выхода на ПРСТ: (2.1, 3.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetPreliminaryStartArrivalMoment()
        {
            var sumDelay = 0;
            // Если обработка не нужна
            if (!ProcessingIsNeeded)
            {
                // Вызываем метод (1.1), прибавляем время движения от стоянки до ПРСТ. Возвращаем результат;
                sumDelay = GetPreliminaryStartDelay() + Intervals[Enums.Intervals.ParkingPreliminaryStartMotion];
                // Создаем момент и возвращаем
                return new Moment(sumDelay);
            }

            // Если обработка нужна
            // вызываем метод (1.2), прибавляем время движения от стоянки до спец площадки,
            // прибавляем время обработки и время движения от спец площадки до ПРСТ
            sumDelay = GetProcessingAndSafeMergeDelay() + Intervals[Enums.Intervals.ParkingSpecPlatformMotion] +
                       Intervals[Enums.Intervals.AircraftProcessing] +
                       Intervals[Enums.Intervals.SpecPlatformPreliminaryStartMotion];
            // Создаем момент и возвращаем
            return new Moment(sumDelay);
        }

        /// <summary>
        /// Расчет момента запуска двигателей: (1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetEngineStartMoment()
        {
            var sumDelay = 0;
            // Если обработка не нужна
            if (!ProcessingIsNeeded)
            {
                // Вызываем метод (1.1), прибавляем момент появления
                sumDelay = GetPreliminaryStartDelay() + Moments[Enums.Moments.Appearance].Value;
                // Создаем момент и возвращаем
                return new Moment(sumDelay);
            }
            // Если обработка нужна => вызываем метод (1.2), прибавляем момент появления
            sumDelay = GetProcessingAndSafeMergeDelay() + Moments[Enums.Moments.Appearance].Value;
            // Создаем момент и возвращаем
            return new Moment(sumDelay);
        }

        /// <summary>
        /// Расчет момента выхода на исполнительный старт (без учета задержек): (1.1.1)
        /// </summary>
        /// <returns></returns>
        private int GetExecutiveStartArrivalMoment()
        {
            // Возвращаем сумму = время движения от стоянки до ПРСТ + время руления на ИСПСТ;
            return Intervals[Enums.Intervals.ParkingPreliminaryStartMotion] +
                   Intervals[Enums.Intervals.ExecutiveStartMotion];
        }

        /// <summary>
        /// Расчет момента взлета (без учета задержек): (1.2.1)
        /// </summary>
        /// <returns></returns>
        private int GetTakeOffMoment()
        {
            // Возвращаем сумму = возврат метода (1.1.1) + время взлета;
            return GetExecutiveStartArrivalMoment() +
                   Intervals[Enums.Intervals.TakeOff];
        }

        /// <summary>
        /// Расчет момента прибытия на {Спец площадку} (без учета задержек): (1.2.1)
        /// </summary>
        /// <returns></returns>
        private int GetSpecPlatformArrivalMoment()
        {
            // Возвращаем время движения от стоянки до Спец площадки;
            return Intervals[Enums.Intervals.ParkingSpecPlatformMotion];
        }

        /// <summary>
        /// Расчет момента покидания {Спец площадки} (без учета задержек): (1.2.2)
        /// </summary>
        /// <returns></returns>
        private int GetSpecPlatformLeaveMoment()
        {
            // Возвращаем сумму = возврат метода (1.2.1) + время обработки;
            return GetSpecPlatformArrivalMoment() + Intervals[Enums.Intervals.AircraftProcessing];
        }
    }
}
