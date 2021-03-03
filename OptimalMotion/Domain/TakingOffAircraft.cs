using System;
using System.Collections.Generic;
using System.Windows.Markup;
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
            ProcessingIsNeeded = creationData.ProcessingIsNeeded;
        }

        private readonly IRunway runway;
        private readonly ISpecPlatform specPlatform;


        public IAircraftId Id { get; set; }
        public Dictionary<Moments, IMoment> Moments { get; }
        public Dictionary<Intervals, int> Intervals { get; }
        public bool ProcessingIsNeeded { get; }

        public int GetRunwayId()
        {
            return runway.Id;
        }

        public int GetSpecPlatformId()
        {
            return specPlatform.Id;
        }

        /// <summary>
        /// Возврат интервала занимания {ВПП} (уже с учетом задержек, чтобы записать в {ВПП}
        /// </summary>
        /// <returns></returns>
        public IInterval GetRunwayOccupationInterval()
        {
            // Вызываем метод (2), получаем момент прибытия на ИСПСТ;
            var startMoment = GetDelayedESArrivalMoment();
            // Вызываем метод (3), получаем момент взлета;
            var endMoment = GetDelayedTakeOffMoment();
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
            var startMoment = GetDelayedSpecPlatformArrivalMoment();
            // К полученному моменту прибавляем время обработки. Получили момент освобождения;
            var endMoment = GetDelayedSpecPlatformLeaveMoment();

            // Формируем интервал и возвращаем его;
            return new Interval(startMoment, endMoment);
        }

        private IMoment GetDelayedSpecPlatformArrivalMoment()
        {
            return new Moment(Moments[Enums.Moments.Appearance].Value + GetProcessingAndSafeMergeDelay() +
                              Intervals[Enums.Intervals.ParkingSpecPlatformMotion]);
        }

        private IMoment GetDelayedSpecPlatformLeaveMoment()
        {
            var leaveMoment = new Moment(GetDelayedSpecPlatformArrivalMoment().Value + Intervals[Enums.Intervals.Processing]);
            Moments[Enums.Moments.SpecPlatformLeave] = leaveMoment;
            return Moments[Enums.Moments.SpecPlatformLeave];
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
                var test = GetPreliminaryStartDelay();
                // Вызываем метод (1.1), прибавляем момент появления
                sumDelay = Moments[Enums.Moments.Appearance].Value + test;

                Moments[Enums.Moments.EngineStart] = new Moment(sumDelay);
                // Создаем момент и возвращаем
                return Moments[Enums.Moments.EngineStart];
            }
            var test1 = GetProcessingAndSafeMergeDelay();

            // Если обработка нужна => вызываем метод (1.2), прибавляем момент появления
            sumDelay = Moments[Enums.Moments.Appearance].Value + test1;

            Moments[Enums.Moments.EngineStart] = new Moment(sumDelay);

            // Создаем момент и возвращаем
            return Moments[Enums.Moments.EngineStart];
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
                specPlatform.GetProcessingAndSafeMergeDelay(aircraftInterval, Intervals[Enums.Intervals.MaxSafeMerge]);

            Intervals[Enums.Intervals.ProcessingWaiting] = processingAndSafeMergeDelay.Item1;
            Intervals[Enums.Intervals.SafeMergeWaiting] = processingAndSafeMergeDelay.Item2;

            var sumOfDelays = Intervals[Enums.Intervals.ProcessingWaiting] +
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
        /// Расчет момента выхода на ПРСТ (с учетом задержки): (2.1, 3.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetDelayedPSArrivalMoment()
        {
            var sumDelay = 0;
            // Если обработка не нужна
            if (!ProcessingIsNeeded)
            {
                var test = GetPreliminaryStartDelay();

                // Вызываем метод (1.1), прибавляем время движения от стоянки до ПРСТ. Возвращаем результат;
                sumDelay = Moments[Enums.Moments.Appearance].Value + test + 
                           Intervals[Enums.Intervals.ParkingPreliminaryStartMotion];

                // Сохраняем момент прибытия на ПРСТ с учетом задержек
                Moments[Enums.Moments.PreliminaryStartArrival] = new Moment(sumDelay);
                // Создаем момент и возвращаем
                return Moments[Enums.Moments.PreliminaryStartArrival];
            }

            // Если обработка нужна
            // Вызываем метод (1.2), прибавляем к моменту появления результат метод (1.2),
            // прибавляем время движения от стоянки до спец площадки,
            // время обработки и время движения от спец площадки до ПРСТ
            var test1 = GetProcessingAndSafeMergeDelay();

            sumDelay = Moments[Enums.Moments.Appearance].Value + test1 +
                       Intervals[Enums.Intervals.ParkingSpecPlatformMotion] +
                       Intervals[Enums.Intervals.Processing] +
                       Intervals[Enums.Intervals.SpecPlatformPreliminaryStartMotion];

            // Сохраняем момент прибытия на ПРСТ с учетом задержек
            Moments[Enums.Moments.PreliminaryStartArrival] = new Moment(sumDelay);
            // Создаем момент и возвращаем
            return Moments[Enums.Moments.PreliminaryStartArrival];
        }

        

        /// <summary>
        /// Расчет момента выхода на исполнительный старт (без учета задержек): (1.1.1)
        /// </summary>
        /// <returns></returns>
        private IMoment GetExecutiveStartArrivalMoment()
        {
            // Если обработка нужна
            if (ProcessingIsNeeded)
                // // Возвращаем сумму = момент появления + время движения от стоянки до Спец площадки +
                // время обработки + время движения от Спец площадки до ПРСТ + время руления на ИСПСТ;
                return new Moment(GetSpecPlatformArrivalMoment().Value + Intervals[Enums.Intervals.Processing] + 
                                  Intervals[Enums.Intervals.SpecPlatformPreliminaryStartMotion] +
                                  Intervals[Enums.Intervals.ExecutiveStartMotion]);

            // Если обработка не нужна
            // Возвращаем сумму = момент появления + время движения от стоянки до ПРСТ + время руления на ИСПСТ;
            return new Moment(Moments[Enums.Moments.Appearance].Value + Intervals[Enums.Intervals.ParkingPreliminaryStartMotion] +
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
            return new Moment(Moments[Enums.Moments.Appearance].Value + Intervals[Enums.Intervals.ParkingSpecPlatformMotion]);
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
