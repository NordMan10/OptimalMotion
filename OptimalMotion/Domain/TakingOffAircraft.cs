using System;
using System.Collections.Generic;
using OptimalMoving.Enums;

namespace OptimalMoving.Domain
{
    public class TakingOffAircraft : ITakingOffAircraft
    {
        public TakingOffAircraft(ITakingOffAircraftCreationData creationData)
        {
            Id = creationData.Id;
            Moments = creationData.CreationMoments.Moments;
            Intervals = creationData.CreationIntervals.Intervals;
            runway = creationData.Runway;
            specPlatform = creationData.SpecPlatform;
            maxProcessingWaitingTime = creationData.MaxProcessingWaitingTime;
            maxPreliminaryStartWaitingTime = creationData.MaxPreliminaryStartWaitingTime;
            safeMergeValue = creationData.SafeMergeValue;
            ProcessingIsNeeded = creationData.ProcessingIsNeeded;
        }

        private readonly IRunway runway;
        private readonly ISpecPlatform specPlatform;
        private readonly int maxProcessingWaitingTime;
        private readonly int maxPreliminaryStartWaitingTime;
        private readonly int safeMergeValue;


        public IAircraftId Id { get; set; }
        public Dictionary<Moments, IMoment> Moments { get; }
        public Dictionary<Intervals, int> Intervals { get; }
        public bool ProcessingIsNeeded { get; }
        
        

        public IInterval GetRunwayOccupationInterval()
        {
            // Вызываем метод (2), получаем момент прибытия на ИСПСТ;
            var startMoment = GetDelayedESArrivalMoment();
            // Вызываем метод (3), получаем момент взлета;
            var endMoment = GetTakeOffMoment();
            // Формируем интервал и возвращаем его;
            return new Interval(startMoment, endMoment);
        }

        /// <summary>
        /// Возврат интервала занимания {Спец площадки} (уже с учетом задержек, чтобы записать в {Спец площадку}): (ИНТЕРФЕЙС) (И.3) 
        /// </summary>
        /// <returns></returns>
        public IInterval GetSpecPlatformOccupationInterval()
        {
            // Если обработка не нужна — выбрасываем исключение с соответствующим сообщением;
            if (!ProcessingIsNeeded)
                throw new Exception("Данный метод может быть вызван только при необходимости обработки");

            // Вызываем метод (1.2) и прибавляем время движения от стоянки до спец. площадки. Получили момент прибытия;
            var startMoment = new Moment(GetProcessingAndSafeMergeDelay() +
                                         Intervals[Enums.Intervals.ParkingSpecPlatformMotion]);
            // К полученному моменту прибавляем время обработки. Получили момент освобождения;
            var endMoment = new Moment(startMoment.Value + Intervals[Enums.Intervals.Processing]);

            // Формируем интервал и возвращаем его;
            return new Interval(startMoment, endMoment);
        }

        public bool IsPlannedMomentFeasible(IPlannedMomentFeasibilityController controller)
        {
            if (ProcessingIsNeeded)
                return controller.IsFeasibleWithProcessing(Moments[Enums.Moments.Appearance],
                    Intervals[Enums.Intervals.ParkingSpecPlatformMotion], maxProcessingWaitingTime,
                    Intervals[Enums.Intervals.Processing],
                    Intervals[Enums.Intervals.SpecPlatformPreliminaryStartMotion],
                    maxPreliminaryStartWaitingTime, Moments[Enums.Moments.PlannedPreliminaryStartArrival]);
                
            return controller.IsFeasibleWithoutProcessing(Moments[Enums.Moments.Appearance],
                    Intervals[Enums.Intervals.ParkingPreliminaryStartMotion], maxPreliminaryStartWaitingTime,
                    Moments[Enums.Moments.PlannedPreliminaryStartArrival]);
        }

        /// <summary>
        /// Расчет момента запуска двигателей: (1)
        /// </summary>
        /// <returns></returns>
        public IMoment GetEngineStartMoment()
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
        /// Расчет интервала ожидания на ПРСТ: (1.1)
        /// </summary>
        /// <returns></returns>
        private int GetPreliminaryStartDelay()
        {
            var preliminaryStartDelay = runway.GetPreliminaryStartMinDelayTime(GetExecutiveStartArrivalMoment(),
                GetTakeOffMoment());
            Intervals[Enums.Intervals.PreliminaryStartWaiting] = preliminaryStartDelay;
            return Intervals[Enums.Intervals.PreliminaryStartWaiting];
        }

        /// <summary>
        /// Расчет суммы интервала ожидания обработки и интервала для безопасного слияния: (1.2)
        /// </summary>
        /// <returns></returns>
        private int GetProcessingAndSafeMergeDelay()
        {
            var aircraftInterval = new Interval(GetSpecPlatformArrivalMoment(),
                GetSpecPlatformLeaveMoment());
            var processingAndSafeMergeDelay =
                specPlatform.GetProcessingAndSafeMergeDelay(aircraftInterval, safeMergeValue);

            Intervals[Enums.Intervals.AircraftProcessingWaiting] = processingAndSafeMergeDelay.Item1;
            Intervals[Enums.Intervals.SafeMergeWaiting] = processingAndSafeMergeDelay.Item2;

            var sumOfDelays = Intervals[Enums.Intervals.AircraftProcessingWaiting] +
                              Intervals[Enums.Intervals.SafeMergeWaiting];

            return sumOfDelays;
        }

        /// <summary>
        /// Расчет момента выхода на исполнительный старт (уже с учетом задержек): (2)
        /// </summary>
        /// <returns></returns>
        private IMoment GetDelayedESArrivalMoment()
        {
            // Вызываем метод (2.1), прибавляем временя руления на исп. старт и возвращаем сумму;
            return new Moment(GetDelayedPSArrivalMoment().Value + Intervals[Enums.Intervals.ExecutiveStartMotion]);
        }

        /// <summary>
        /// Расчет момента взлета (покидания ВПП): (3)
        /// </summary>
        /// <returns></returns>
        private IMoment GetDelayedTakeOffMoment()
        {
            // Вызываем метод (2), прибавляем временя взлета. Возвращаем сумму;
            return new Moment(GetDelayedESArrivalMoment().Value + Intervals[Enums.Intervals.TakeOff]);
        }

        /// <summary>
        /// Расчет момента выхода на ПРСТ: (2.1, 3.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetDelayedPSArrivalMoment()
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
                       Intervals[Enums.Intervals.Processing] +
                       Intervals[Enums.Intervals.SpecPlatformPreliminaryStartMotion];
            // Создаем момент и возвращаем
            return new Moment(sumDelay);
        }

        

        /// <summary>
        /// Расчет момента выхода на исполнительный старт (без учета задержек): (1.1.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetExecutiveStartArrivalMoment()
        {
            // Возвращаем сумму = время движения от стоянки до ПРСТ + время руления на ИСПСТ;
            return new Moment(Intervals[Enums.Intervals.ParkingPreliminaryStartMotion] +
                   Intervals[Enums.Intervals.ExecutiveStartMotion]);
        }

        /// <summary>
        /// Расчет момента взлета (без учета задержек): (1.2.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetTakeOffMoment()
        {
            // Возвращаем сумму = возврат метода (1.1.1) + время взлета;
            return new Moment(GetExecutiveStartArrivalMoment().Value +
                   Intervals[Enums.Intervals.TakeOff]);
        }

        /// <summary>
        /// Расчет момента прибытия на {Спец площадку} (без учета задержек): (1.2.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetSpecPlatformArrivalMoment()
        {
            // Возвращаем время движения от стоянки до Спец площадки;
            return new Moment(Intervals[Enums.Intervals.ParkingSpecPlatformMotion]);
        }

        /// <summary>
        /// Расчет момента покидания {Спец площадки} (без учета задержек): (1.2.2)
        /// </summary>
        /// <returns></returns>
        private IMoment GetSpecPlatformLeaveMoment()
        {
            // Возвращаем сумму = возврат метода (1.2.1) + время обработки;
            return new Moment(GetSpecPlatformArrivalMoment().Value + Intervals[Enums.Intervals.Processing]);
        }
    }
}
