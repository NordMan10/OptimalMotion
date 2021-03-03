using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public class SpecPlatform : ISpecPlatform
    {
        public SpecPlatform(int id)
        {
            Id = id;
            OccupationIntervals = new Dictionary<IMoment, IMoment>();
        }

        public int Id { get; }
        public Dictionary<IMoment, IMoment> OccupationIntervals { get; }

        /// <summary>
        /// Метод, возвращающий задержку для ожидания обработки и для безопасного слияния (в кортеже)
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> GetProcessingAndSafeMergeDelay(IInterval aircraftInterval, int safeMergeValueParam)
        {
            // Принимаем интервал занимания Спец площадки и задержку для безопасного слияния от обратившегося судна
            var currentInterval = aircraftInterval;
            var safeMergeValue = safeMergeValueParam;

            // Получаем расширенный интервал:
            // Получаем начальный момент расширенного интервала:
            var extendedIntervalStartMoment = currentInterval.StartMoment.Value - safeMergeValue;
            if (extendedIntervalStartMoment < 0)
                extendedIntervalStartMoment = 0;
            // Создаем расширенный интервал
            var extendedInterval = new Interval(new Moment(extendedIntervalStartMoment),
                new Moment(currentInterval.EndMoment.Value + safeMergeValue));

            // Получаем левый и правый интервалы (метод ЗПД) относительно принятого интервала;
            var leftAndRightIntervals = this.GetLeftAndRightIntervalsRelative(currentInterval);
            var leftInterval = leftAndRightIntervals.Item1;
            var rightInterval = leftAndRightIntervals.Item2;

            // Проверяем пересечение расширенного интервала с ближайшими левым и правым интервалом:
            // Если пересечения нет => возвращаем два нуля;
            if (leftInterval == null || !this.DoesIntervalsIntersect(extendedInterval, leftInterval))
                if (rightInterval == null || !this.DoesIntervalsIntersect(extendedInterval, rightInterval))
                    return Tuple.Create(0, 0);

            // Если пересечение есть, то рассчитываем задержки
            return GetDelaysForNewLastAircraft(currentInterval, safeMergeValue);

            // Пока что сделаем расчет задержек упрощенным, потом сюда вернемся.ы
            //// Если есть => проверяем пересечение для изначального интервала:
            //// Если нет пересечения у изначального интервала => 
            //if (rightInterval == null || !this.DoesIntervalsIntersect(currentInterval, rightInterval))
            //    if (leftInterval == null || !this.DoesIntervalsIntersect(currentInterval, leftInterval))
            //        // Рассчитываем задержку для безопасного слияния
            //        if ()
        }

        /// <summary>
        /// Метод расчета задержки для безопасного слияния
        /// </summary>
        /// <param name="leftIntervalParam"></param>
        /// <param name="rightIntervalParam"></param>
        /// <param name="safeMergeValueParam"></param>
        /// <returns></returns>
        private int GetSafeMergeDelay(IInterval leftIntervalParam, IInterval rightIntervalParam, int safeMergeValueParam)
        {
            // Принимаем два интервала и значение интервала для безопасного слияния  
            var leftInterval = leftIntervalParam;
            var rightInterval = rightIntervalParam;
            var safeMergeValue = safeMergeValueParam;

            // Вычисляем модуль разности между начальными моментами этих интервалов;
            var startMomentsDifference = Math.Abs(leftInterval.StartMoment.Value - rightInterval.StartMoment.Value);

            // Если полученная разность >= интервалу для безопасного слияния => возвращаем ноль;
            if (startMomentsDifference >= safeMergeValue)
                return 0;

            // Если нет => возвращаем интервал для безопасного слияния = значение
            // интервала для безопасного слияния - рассчитанная разность;
            return safeMergeValue - startMomentsDifference;
        }

        /// <summary>
        /// Метод сдвига интервала
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        private IInterval ShiftInterval(IInterval intervalParam, int shiftParam)
        {
            // 1) Принимаем интервал и значение сдвига;
            var interval = intervalParam;
            var shift = shiftParam;

            // 2) Увеличиваем значение начального и конечного момента на переданное значение;
            var shiftedInterval = new Interval(new Moment(interval.StartMoment.Value + shift),
                new Moment(interval.EndMoment.Value + shift), interval.Type);

            // 3) Возвращаем новый интервал;
            return shiftedInterval;
        }

        /// <summary>
        /// Метод, возвращающий задержки при добавлении нового судна в конец очереди
        /// </summary>
        /// <param name="aircraftInterval"></param>
        /// <param name="safeMergeValueParam"></param>
        /// <returns></returns>
        private Tuple<int, int> GetDelaysForNewLastAircraft(IInterval aircraftInterval, int safeMergeValueParam)
        {
            // Принимаем интервал обратившегося судна;
            var currentInterval = aircraftInterval;
            var safeMergeValue = safeMergeValueParam;

            // Получаем начальный момент последнего записанного судна
            var lastWrittenStartMoment = OccupationIntervals.Keys.OrderBy(key => key).Last();
            // Получаем конечный момент последнего записанного судна
            var lastWrittenEndMoment = OccupationIntervals[lastWrittenStartMoment];

            // Сохраняем интервал ожидания обработки = момент покидания площадки последним записанным судном 
            // минус момент прибытия (без задержки) обратившегося судна;
            var processingDelay = lastWrittenEndMoment.Value - currentInterval.StartMoment.Value;

            // Сдвигаем текущий интервал на полученную задержку
            var shiftedCurrentInterval = ShiftInterval(currentInterval, processingDelay);

            // Получаем задержку для соблюдения интервала безопасного слияния
            var safeMergeDelay = GetSafeMergeDelay(new Interval(lastWrittenStartMoment, lastWrittenEndMoment),
                shiftedCurrentInterval, safeMergeValue);

            // 4) Возвращаем в кортеже задержку для ожидания обработки и задержку для безопасного слияния;
            return Tuple.Create(processingDelay, safeMergeDelay);
        }
    }
}
